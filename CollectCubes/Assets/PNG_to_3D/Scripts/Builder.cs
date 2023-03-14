using System.Collections;
using UnityEngine;
using cky.Pooling;
using cky.Helpers;
using cky.Managers;

namespace cky.PngTo3D.Builder
{
    public class Builder : MonoBehaviour
    {
        #region Variables

        LevelSettings _s;
        MaterialHolderInfo _m;
        PoolManager _poolManager;

        Vector3 _boxDistance;
        Vector3 _readPixelStepVector3;
        Color[,] pixels;
        Vector3 vector3One = Vector3.one;
        int _textureHeight;
        int _textureWidth;
        int _requiredMaterialCount = 0;
        float _materialCheatBound;
        float _height;

        int _maxBoxCount;
        int _targetBoxCount;
        int _currentBoxCount = 0;
        int _targetX, _targetY;

        #endregion

        private void Start()
        {
            EventManager.CreateBuildingFromPNG += Create;
        }

        private void Initialize()
        {
            Reset();

            //_s = Resources.Load<LevelSettings>($"{_objectAdress}/{objectName}");
            _s = LevelManager.Instance.levelSettings;

            _m = _s.materialHolderInfo;
            _materialCheatBound = _s.MaterialCheatBound;
            _m.AtStart();

            _poolManager = PoolManager.instance;
            _boxDistance = _s.brickPrefabTr.localScale * _s.BoxSize;
        }

        private void Reset()
        {
            _maxBoxCount = 0;
            _targetBoxCount = 0;
            _currentBoxCount = 0;
        }
        public void Create()
        {
            Initialize();

            _height = _s.BoxSize * 0.5f;
            _readPixelStepVector3 = 1 * _s.PixelStep * _boxDistance;
            _textureHeight = _s.TextureMap.height;
            _textureHeight = _textureHeight / (int)_readPixelStepVector3.y;
            _textureWidth = _s.TextureMap.width;
            _textureWidth = _textureWidth / (int)_readPixelStepVector3.x;
            pixels = GetPixelTextures.ReadTextureMap(_s.TextureMap, _s.PixelStep, _boxDistance);

            Build(pixels, _textureWidth, _textureHeight);
        }

        private void Build(Color[,] pixels, int textureWidth, int textureHeight)
        {
            FindMaxBoxCount(pixels, textureWidth, textureHeight);

            _targetBoxCount = Mathf.RoundToInt(_maxBoxCount * ((float)_s.occupancyPercentage / 100));

            StartCoroutine(Generate(pixels, textureWidth, textureHeight));
        }

        #region Generating

        IEnumerator Generate(Color[,] pixels, int textureWidth, int textureHeight)
        {
            yield return new WaitForSeconds(0.1f);

            for (int y = 0; y < textureHeight; y++)
            {
                for (int x = 0; x < textureWidth; x++)
                {
                    if (pixels[x, y].a > 0.5f)
                    {
                        Vector3 pos = transform.position + new Vector3(_boxDistance.x * (x - textureWidth / 2), _height, _boxDistance.y * y);
                        Transform brickTr = _poolManager.Spawn(_s.brickPrefabTr, pos, RandomRotation()).transform;

                        SetBox(brickTr, x, y);

                        _currentBoxCount++;

                        if (IsOnTargetPercentage() == true)
                        {
                            FindNext_XY(pixels, textureWidth, textureHeight, x, y);

                            cky.Managers.EventManager.Instance.SetTargetScoreEvent(_currentBoxCount);

                            print($"png's %{_s.occupancyPercentage} created. (Can Adjust from Level Settings.)");

                            yield break;
                        }
                    }
                }

                if (_currentBoxCount % 100 == 99)
                    yield return null;
            }

            print($"Req Mat{_requiredMaterialCount}, Req Box {_currentBoxCount}");
        }

        private void SetBox(Transform box, int x, int y)
        {
            box.gameObject.layer = LayerHelper.FREECUBE;
            box.localScale = _s.BoxSize * vector3One;
            MeshRenderer mr = box.GetComponent<MeshRenderer>();
            //box.GetComponent<Rigidbody>().isKinematic = true;

            Material cached = null;

            cached = ReturnCachedMaterialIfCachedBefore(x, y);

            if (cached != null) mr.material = cached;
            else
            {
                mr.material = _m.GetMaterialWithIndex(_m.AddNewColor(pixels[x, y]));
                _requiredMaterialCount++;
            }
        }

        private Material ReturnCachedMaterialIfCachedBefore(int x, int y)
        {
            foreach (var cachedM in _m.cachedMats)
            {
                Color cachedMatColor = cachedM.color;
                if (cachedMatColor.a == 0) continue;
                if (cachedMatColor.r > pixels[x, y].r + _materialCheatBound) continue;
                if (cachedMatColor.r < pixels[x, y].r - _materialCheatBound) continue;
                if (cachedMatColor.g > pixels[x, y].g + _materialCheatBound) continue;
                if (cachedMatColor.g < pixels[x, y].g - _materialCheatBound) continue;
                if (cachedMatColor.b > pixels[x, y].b + _materialCheatBound) continue;
                if (cachedMatColor.b < pixels[x, y].b - _materialCheatBound) continue;
                return cachedM;
            }
            return null;
        }

        Quaternion RandomRotation()
        {
            return Quaternion.Euler(90,
                Random.Range(-_s.randomRotFactor, _s.randomRotFactor),
                Random.Range(-_s.randomRotFactor, _s.randomRotFactor));
        }

        #endregion

        #region Generate With Order

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                GenerateNextBox(pixels, _textureWidth, _textureHeight);
        }

        private void GenerateNextBox(Color[,] pixels, int textureWidth, int textureHeight)
        {
            Vector3 pos = transform.position + new Vector3(_boxDistance.x * (_targetX - textureWidth / 2), _height, _boxDistance.y * _targetY);
            Transform brickTr = _poolManager.Spawn(_s.brickPrefabTr, pos, RandomRotation()).transform;

            SetBox(brickTr, _targetX, _targetY);

            _currentBoxCount++;

            FindNext_XY(pixels, textureWidth, textureHeight, _targetX, _targetY);

            //print($"Req Mat{_requiredMaterialCount}, Req Box {_currentBoxCount}");
        }

        private void FindNext_XY(Color[,] pixels, int textureWidth, int textureHeight, int _x, int _y)
        {
            int counter = 0;

            for (int y = _y; y < textureHeight; y++)
            {
                if (counter != 0)
                    _x = 0;

                for (int x = _x; x < textureWidth; x++)
                {
                    if (pixels[x, y].a > 0.5f)
                    {
                        counter++;
                        if (counter == 2)
                        {
                            _targetX = x;
                            _targetY = y;
                            //print($"{x}  {y}");

                            break;
                        }
                    }
                }
            }
        }

        #endregion

        #region Find Minimum Needed Box In Pool

        public void FindMaxBoxCount(Color[,] pixels, int textureWidth, int textureHeight)
        {
            for (int y = 0; y < textureHeight; y++)
                for (int x = 0; x < textureWidth; x++)
                    if (pixels[x, y].a > 0.5f)
                        _maxBoxCount++;

            print($"{_maxBoxCount}");
        }

        #endregion

        #region Conditions

        private bool IsOnTargetPercentage()
        {
            return _currentBoxCount == _targetBoxCount;
        }

        #endregion
    }
}