using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AlchemyRecipe
{
    public string[] ingredients; // size = 3
    public ItemData result;
}

[CreateAssetMenu(fileName = "Alchemy Recipe", menuName = "Recipe Data", order = 49)]
public class AlchemyRecipeData : ScriptableObject
{
    public static AlchemyRecipeData Instance;

    private void OnEnable() => Instance = this;

    public List<AlchemyRecipe> recipes;

    public ItemData GetResult(string[] input)
    {
        foreach (var r in recipes)
        {
            // so sánh không cần đúng thứ tự
            if (IsMatch(r.ingredients, input))
                return r.result;
        }
        return null;
    }

    bool IsMatch(string[] a, string[] b)
    {
        var listA = new List<string>(a);
        var listB = new List<string>(b);

        for (int i = 0; i < listA.Count; i++)
            listA[i] = listA[i]?.Trim().ToLower() ?? "";
        for (int i = 0; i < listB.Count; i++)
            listB[i] = listB[i]?.Trim().ToLower() ?? "";

        listA.Sort();
        listB.Sort();
        for (int i = 0; i < 3; i++)
        {
            if (listA[i] != listB[i]) return false;
        }
        return true;
    }
}
