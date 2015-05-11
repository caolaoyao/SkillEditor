using UnityEditor;
using UnityEngine;

// [CustomEditor(typeof(TreeViewControl))]
public class TreeViewInspector : Editor
{
    static TreeViewControl item = null;
    public static TreeViewControl AddTreeView()
    {
        item = new TreeViewControl();
        AssignDefaults(item);
        return item;
    }

    public static void AssignDefaults(TreeViewControl item)
    {
        // create new skin instance
        GUISkin skinHover = ScriptableObject.CreateInstance<GUISkin>();
        GUISkin skinSelected = ScriptableObject.CreateInstance<GUISkin>();
        GUISkin skinUnselected = ScriptableObject.CreateInstance<GUISkin>();

        // name the skins
        skinHover.name = "Hover";
        skinSelected.name = "Selected";
        skinUnselected.name = "Unselected";

        item.m_textureBlank = GetTexture("Assets/TreeViewControl/blank.png");
        item.m_textureGuide = GetTexture("Assets/TreeViewControl/guide.png");
        item.m_textureLastSiblingCollapsed = GetTexture("Assets/TreeViewControl/last_sibling_collapsed.png");
        item.m_textureLastSiblingExpanded = GetTexture("Assets/TreeViewControl/last_sibling_expanded.png");
        item.m_textureLastSiblingNoChild = GetTexture("Assets/TreeViewControl/last_sibling_nochild.png");
        item.m_textureMiddleSiblingCollapsed = GetTexture("Assets/TreeViewControl/middle_sibling_collapsed.png");
        item.m_textureMiddleSiblingExpanded = GetTexture("Assets/TreeViewControl/middle_sibling_expanded.png");
        item.m_textureMiddleSiblingNoChild = GetTexture("Assets/TreeViewControl/middle_sibling_nochild.png");
        item.m_textureNormalChecked = GetTexture("Assets/TreeViewControl/normal_checked.png");
        item.m_textureNormalUnchecked = GetTexture("Assets/TreeViewControl/normal_unchecked.png");
        item.m_textureSelectedBackground = GetTexture("Assets/TreeViewControl/selected_background_color.png");
        item.m_skinHover = skinHover;
        item.m_skinSelected = skinSelected;
        item.m_skinUnselected = skinUnselected;

        SetBackground(item.m_skinHover.button, null);
        SetBackground(item.m_skinHover.toggle, null);
        SetButtonFontSize(item.m_skinHover.button);
        SetButtonFontSize(item.m_skinHover.toggle);
        RemoveMargins(item.m_skinHover.button);
        RemoveMargins(item.m_skinHover.toggle);
        SetTextColor(item.m_skinHover.button, Color.yellow);
        SetTextColor(item.m_skinHover.toggle, Color.yellow);

        SetBackground(item.m_skinSelected.button, item.m_textureSelectedBackground);
        SetBackground(item.m_skinSelected.toggle, item.m_textureSelectedBackground);
        SetButtonFontSize(item.m_skinSelected.button);
        SetButtonFontSize(item.m_skinSelected.toggle);
        RemoveMargins(item.m_skinSelected.button);
        RemoveMargins(item.m_skinSelected.toggle);
        SetTextColor(item.m_skinSelected.button, Color.yellow);
        SetTextColor(item.m_skinSelected.toggle, Color.yellow);

        SetBackground(item.m_skinUnselected.button, null);
        SetBackground(item.m_skinUnselected.toggle, null);
        SetButtonFontSize(item.m_skinUnselected.button);
        SetButtonFontSize(item.m_skinUnselected.toggle);
        RemoveMargins(item.m_skinUnselected.button);
        RemoveMargins(item.m_skinUnselected.toggle);
        SetTextColor(item.m_skinUnselected.button, Color.white);
        SetTextColor(item.m_skinUnselected.toggle, Color.white);
    }

	static void SetBackground(GUIStyle style, Texture2D texture)
	{
		style.active.background = texture;
		style.focused.background = texture;
		style.hover.background = texture;
		style.normal.background = texture;
		style.onActive.background = texture;
		style.onFocused.background = texture;
		style.onHover.background = texture;
		style.onNormal.background = texture;
	}
	
	static void SetTextColor(GUIStyle style, Color color)
	{
		style.active.textColor = color;
		style.focused.textColor = color;
		style.hover.textColor = color;
		style.normal.textColor = color;
		style.onActive.textColor = color;
		style.onFocused.textColor = color;
		style.onHover.textColor = color;
		style.onNormal.textColor = color;
	}
	
	static void RemoveMargins(GUIStyle style)
	{
		style.margin.bottom = 0;
		style.margin.left = 0;
		style.margin.right = 0;
		style.margin.top = 0;
	}

    static void SetButtonFontSize(GUIStyle style)
    {
        style.fontSize = 12;
    }

    static Texture2D GetTexture(string texturePath)
    {
        try
        {
            Texture2D item = (Texture2D)AssetDatabase.LoadAssetAtPath(texturePath, typeof(Texture2D));
            return item;
        }
        catch (System.Exception ex)
        {
            Debug.LogError(string.Format("Failed to find local texture: {0}", ex));
            return null;
        }
    }

    static GUISkin GetGUISkin(string skinPath)
    {
        try
        {
            GUISkin item = (GUISkin)AssetDatabase.LoadAssetAtPath(skinPath, typeof(GUISkin));
            return item;
        }
        catch (System.Exception ex)
        {
            Debug.LogError(string.Format("Failed to find local GUI skin: {0}", ex));
            return null;
        }
    }
}
