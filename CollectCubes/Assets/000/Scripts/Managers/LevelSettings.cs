using cky.PngTo3D.Builder;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Level/New Level Settings")]
public class LevelSettings : ScriptableObject
{
    [Header("Player")]
    [Space]
    public float playerMoveSpeed = 10.0f;
    public float playerRotationSpeed = 10.0f;

    [Header("Opponent")]
    [Space]
    public float aiMoveSpeed = 25.0f;
    public float aiTurnSpeed = 10.0f;
    public float aiFindingRadiusStartValue = 3.0f;
    public LayerMask aiTargetMask;

    [Header("Game Settings")]
    [Space]
    public float gameTime = 60.0f;

    [Header("Spawner Settings")]
    [Space]
    public float spawnerInterval = 0.5f;
    public float spawnerThrowPower = 100.0f;
    public float spawnerThrowingMaxValueXZ = 15.0f;
    public float spawnerThrowingMaxValueY = 25.0f;
    public float spawnerThrowingMinValueY = 15.0f;

    [Header("Build Settings")]
    [Space]
    public MaterialHolderInfo materialHolderInfo;
    public Transform brickPrefabTr;
    [Range(0, 100)] public int occupancyPercentage = 100;
    public float randomRotFactor = 10.0f;

    [Range(0, 3)]
    [SerializeField] private float boxSize;
    public float BoxSize { get { return boxSize; } }


    [Header("       TEXTURE")]
    [SerializeField] private Texture2D textureMap;
    public Texture2D TextureMap { get { return textureMap; } }


    [Range(1, 1000)]
    [SerializeField] private int _pixelStep;
    public int PixelStep { get { return _pixelStep; } }

    [Range(0, 1)]
    public float MaterialCheatBound = 0.1f;
}