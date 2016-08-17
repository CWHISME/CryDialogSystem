/**********************************************************
*Author: wangjiaying
*Date: 2016.6.27
*Func:
**********************************************************/
using UnityEngine;

namespace CryDialog.Editor
{
    public class ResourcesManager : Singleton<ResourcesManager>
    {
        //Resources
        public Texture2D texInputSlot;
        public Texture2D texInputSlotActive;
        public Texture2D texOutputSlot;
        public Texture2D texOutputSlotActive;
        public Texture2D texGrid;
        public Texture2D texWhiteBorder;
        public Texture2D texBackground;

        public GUISkin skin;

        public GUIStyle StyleBackground { get { return skin.GetStyle("Background"); } }

        public GUIStyle DefaultNode { get { return skin.GetStyle("DefaultNode"); } }
        public GUIStyle DefaultNodeOn { get { return skin.GetStyle("DefaultNodeOn"); } }
        public GUIStyle EventNode { get { return skin.GetStyle("EventNode"); } }
        public GUIStyle EventNodeOn { get { return skin.GetStyle("EventNodeOn"); } }
        public GUIStyle ConditionNode { get { return skin.GetStyle("ConditionNode"); } }
        public GUIStyle ConditionNodeOn { get { return skin.GetStyle("ConditionNodeOn"); } }
        public GUIStyle ActionNode { get { return skin.GetStyle("ActionNode"); } }
        public GUIStyle ActionNodeOn { get { return skin.GetStyle("ActionNodeOn"); } }

        private GUIStyle fontStyle = new GUIStyle();
        public GUIStyle GetFontStyle(int fontSize)
        {
            fontStyle.normal.textColor = Color.white;
            fontStyle.fontSize = fontSize;
            return fontStyle;
        }

        public GUIStyle GetOverflowFontStyle(int fontSize)
        {
            GUIStyle style = GetFontStyle((int)(fontSize * Tools.Zoom));
            style.clipping = TextClipping.Overflow;
            style.wordWrap = true;
            return style;
        }

        public GUIStyle GetFontStyle(int fontSize, Color color)
        {
            fontStyle.normal.textColor = color;
            fontStyle.fontSize = fontSize;
            return fontStyle;
        }

        public ResourcesManager()
        {
            if (texInputSlot == null)
                texInputSlot = Resources.Load("Image/Slot/input_slot") as Texture2D;
            if (texInputSlotActive == null)
                texInputSlotActive = Resources.Load("Image/Slot/input_slot_active") as Texture2D;
            if (texOutputSlot == null)
                texOutputSlot = Resources.Load("Image/Slot/output_slot") as Texture2D;
            if (texOutputSlotActive == null)
                texOutputSlotActive = Resources.Load("Image/Slot/output_slot_active") as Texture2D;
            if (texGrid == null)
                texGrid = Resources.Load("Image/grid") as Texture2D;
            if (texWhiteBorder == null)
                texWhiteBorder = Resources.Load("WhiteBorder") as Texture2D;
            if (texBackground == null)
                texBackground = Resources.Load("Background") as Texture2D;
            if (skin == null) skin = Resources.Load<GUISkin>("Skin");
        }

    }
}