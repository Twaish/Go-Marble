using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CustomizationRepository : MonoBehaviour {
  public event Action<string> OnMarbleChanged;
  public event Action<string> OnTrailChanged;
  public event Action<List<string>> OnAccessoriesChanged;
  
  private CustomizationData customizationData;
  private CustomizationPersistence persistence;
  private readonly int maxAccessories = 3;

  private void Awake() {
    persistence = new CustomizationPersistence();
    customizationData = persistence.Load();
  }

  public string GetSelectedMarble() => string.IsNullOrEmpty(customizationData.selectedMarble) ? null : customizationData.selectedMarble;
  public string GetSelectedTrail() => string.IsNullOrEmpty(customizationData.selectedTrail) ? null : customizationData.selectedTrail;
  public IReadOnlyList<string> GetSelectedAccessories() => customizationData.selectedAccessories;

  public CustomizationData GetCustomizationData() => customizationData;

  public void SetSelectedMarble(string marbleName) {
    customizationData.selectedMarble = (customizationData.selectedMarble == marbleName) ? null : marbleName;
    Save();
    OnMarbleChanged?.Invoke(customizationData.selectedMarble);
  }

  public void SetSelectedTrail(string trailName) {
    customizationData.selectedTrail = (customizationData.selectedTrail == trailName) ? null : trailName;
    Save();
    OnTrailChanged?.Invoke(customizationData.selectedTrail);
  }

  public void ToggleAccessory(string accessoryName) {
    if (customizationData.selectedAccessories.Contains(accessoryName)) {
      customizationData.selectedAccessories.Remove(accessoryName);
    } else if (customizationData.selectedAccessories.Count < maxAccessories) {
      customizationData.selectedAccessories.Add(accessoryName);
    }
    Save();
    OnAccessoriesChanged?.Invoke(customizationData.selectedAccessories);
  }

  private void Save() => persistence.Save(customizationData);
}

public class CustomizationPersistence {
  private string FilePath => Path.Combine(Application.persistentDataPath, "customization.json");

  public void Save(CustomizationData data) {
    string json = JsonUtility.ToJson(data, true);
    File.WriteAllText(FilePath, json);
  }

  public CustomizationData Load() {
    if (!File.Exists(FilePath)) return new CustomizationData();
    string json = File.ReadAllText(FilePath);
    return JsonUtility.FromJson<CustomizationData>(json);
  }
}

[Serializable]
public class CustomizationData {
  public string selectedMarble;
  public string selectedTrail;
  public List<string> selectedAccessories = new();
}
