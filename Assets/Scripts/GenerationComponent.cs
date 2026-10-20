using UnityEngine;
using Random = UnityEngine.Random;

public class GenerationComponent : MonoBehaviour
{
    private PrefabLibrary _prefabLibrary;

    private void Start()
    {
        _prefabLibrary = Resources.Load<PrefabLibrary>("PrefabLibrary");
    }

    public GameItem GetNextItem()
    {
        var randomCubeType = Constants.AllCubeTypes[Random.Range(0, Constants.AllCubeTypes.Count)];
        var randomCubePrefab = _prefabLibrary.GetGameItemPrefabFromType(randomCubeType);
        var randomCube = Instantiate(
            randomCubePrefab, transform.position + Vector3.up, Quaternion.identity);

        return randomCube;
    }
}
