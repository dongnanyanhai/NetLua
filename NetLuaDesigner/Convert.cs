using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Reflection;

namespace NetLuaDesigner
{
    internal class Convert
    {
        private string curStartCode = "";

        private string startCode = @"import('System')
import('System.Text')
import('System.Drawing')
import('System.Windows')
import('System.Windows.Forms')
import('System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a')
import('System.Text, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a')
import('System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a')
import('System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a')

-- 核心表
local _M = { autoInitialize = true }

-- 功能函数
function _M:InitializeData()
end

-- 处理事件
function _M:Demo_Event(sender, e)
    MessageBox.Show('Demo Event Handler')
end

";
        private string midCode = @"function _M:InitializeComponent()
    -- 从这一行下面开始不要添加业务代码

";

        private string endCode = @"    --不要删除下面的XML代码
    --[[XML--]]

end -- InitializeComponentEnd

function _M:initialize()
    -- 开启风格
    if self.autoInitialize then
        Application.EnableVisualStyles()
        self:InitializeComponent()
        self:InitializeData()
        Application.Run(self.Form1)
    end
end

_M:initialize()";

        public Convert(){

        }

        private static string Property2Font(string font)
        {

            string fontCode = "Font(";

            font.Replace(", style=","|");

            string[] split1 = font.Split('|');

            string[] split2 = split1[0].Split(',');

            fontCode += "\"" + split2[0].Trim() + "\", " + split2[1].Replace("pt","").Trim();

            if(split1.Length == 2){
                fontCode += ", luanet.enum(FontStyle, '" + split1[1] + "')";
            }

            fontCode += ")";

            return fontCode;

        }

        private static string Property2Color(string color)
        {
            string[] systemColorName = new string[] { "ActiveBorder", "ActiveCaption", "ActiveCaptionText", "AppWorkspace", "ButtonFace", "ButtonHighlight", "ButtonShadow", "Control", "ControlDark", "ControlDarkDark", "ControlLight", "ControlLightLight", "ControlText", "Desktop", "GradientActiveCaption", "GradientInactiveCaption", "GrayText", "Highlight", "HighlightText", "HotTrack", "InactiveBorder", "InactiveCaption", "InactiveCaptionText", "Info", "InfoText", "Menu", "MenuBar", "MenuHighlight", "MenuText", "ScrollBar", "Window", "WindowFrame", "WindowText" };

            string[] colorName = new string[] { "Black", "DimGray", "Gray", "DarkGray", "Silver", "LightGray", "Gainsboro", "WhiteSmoke", "White", "RosyBrown", "IndianRed", "Brown", "Firebrick", "LightCoral", "Maroon", "DarkRed", "Red", "Snow", "MistyRose", "Salmon", "Tomato", "DarkSalmon", "Coral", "OrangeRed", "LightSalmon", "Sienna", "SeaShell", "Chocolate", "SaddleBrown", "SandyBrown", "PeachPuff", "Peru", "Linen", "Bisque", "DarkOrange", "BurlyWood", "Tan", "AntiqueWhite", "NavajoWhite", "BlanchedAlmond", "PapayaWhip", "Moccasin", "Orange", "Wheat", "OldLace", "FloralWhite", "DarkGoldenrod", "Goldenrod", "Cornsilk", "Gold", "Khaki", "LemonChiffon", "PaleGoldenrod", "DarkKhaki", "Beige", "LightGoldenrodYellow", "Olive", "Yellow", "LightYellow", "Ivory", "OliveDrab", "YellowGreen", "DarkOliveGreen", "GreenYellow", "Chartreuse", "LawnGreen", "DarkSeaGreen", "ForestGreen", "LimeGreen", "LightGreen", "PaleGreen", "DarkGreen", "Green", "Lime", "Honeydew", "SeaGreen", "MediumSeaGreen", "SpringGreen", "MintCream", "MediumSpringGreen", "MediumAquamarine", "Aquamarine", "Turquoise", "LightSeaGreen", "MediumTurquoise", "DarkSlateGray", "PaleTurquoise", "Teal", "DarkCyan", "Cyan", "Aqua", "LightCyan", "Azure", "DarkTurquoise", "CadetBlue", "PowderBlue", "LightBlue", "DeepSkyBlue", "SkyBlue", "LightSkyBlue", "SteelBlue", "AliceBlue", "DodgerBlue", "SlateGray", "LightSlateGray", "LightSteelBlue", "CornflowerBlue", "RoyalBlue", "MidnightBlue", "Lavender", "Navy", "DarkBlue", "MediumBlue", "Blue", "GhostWhite", "SlateBlue", "DarkSlateBlue", "MediumSlateBlue", "MediumPurple", "BlueViolet", "Indigo", "DarkOrchid", "DarkViolet", "MediumOrchid", "Thistle", "Plum", "Violet", "Purple", "DarkMagenta", "Fuchsia", "Magenta", "Orchid", "MediumVioletRed", "DeepPink", "HotPink", "LavenderBlush", "PaleVioletRed", "Crimson" };

            color = color.Replace("Color [", "").Replace("]", "");
            string strSystemColor = "SystemColors.";
            string strColor = "Color.";
            string strRGB = "Color.FromArgb(";

            if (color == "Transparent")
            {
                return "Color.Transparent";
            }

            for (int i = 0; i < systemColorName.Length; i++)
            {
                if (systemColorName[i] == color)
                {
                    return strSystemColor + color;
                }
            }

            for (int i = 0; i < colorName.Length; i++)
            {
                if (colorName[i] == color)
                {
                    return strColor + color;
                }
            }

            // color = color.Replace("A", "").Replace("R", "").Replace("G", "").Replace("B", "").Replace("=", "");
            string[] split = color.Split(',');
            if(split.Length == 3){
                strRGB += split[0] + "," + split[1] + "," + split[2] + ")";
            }else if(split.Length == 4){
                strRGB += split[0] + "," + split[1] + "," + split[2] + "," + split[4] + ")";
            }
            return strRGB;
        }

        private string AnalysisProperty(XElement curElement, string parentName, PropertyInfo[] parentProps)
        {
            // 当前控件的变量名称
            string propertyName = "";
            string propertyValue = "";
            string resultCode = "";

            // 先获取Name
            propertyName = curElement.Name.ToString();
            // 再获取Value
            propertyValue = curElement.Value.ToString();

            string prevCode = "self." + parentName + "." + propertyName;

            // 另外一种方案
            foreach (var prop in parentProps)
            {
                if(propertyName != prop.Name)
                {
                    continue;
                }

                if(prop.PropertyType == typeof(System.Int32))
                {
                    return prevCode + " = " + int.Parse(propertyValue);
                }

                if(prop.PropertyType == typeof(System.String))
                {
                    return prevCode + " = \"" + propertyValue + "\"";
                }

                if(prop.PropertyType == typeof(System.Boolean))
                {
                    return prevCode + " = " + propertyValue.ToLower();
                }

                if(prop.PropertyType == typeof(System.Windows.Forms.AnchorStyles))
                {
                    return prevCode + " = luanet.enum(AnchorStyles, '" + propertyValue + "')";
                }

                if(prop.PropertyType == typeof(System.Windows.Forms.Padding))
                {
                    return prevCode + " = Padding(" + propertyValue + ")";
                }

                if(prop.PropertyType == typeof(System.Drawing.Point))
                {
                    return prevCode + " = Point(" + propertyValue + ")";
                }

                if(prop.PropertyType == typeof(System.Drawing.Size))
                {
                    return prevCode + " = Size(" + propertyValue + ")";
                }

                if(prop.PropertyType == typeof(System.Drawing.SizeF))
                {
                    return prevCode + " = SizeF(" + propertyValue + ")";
                }

                if(prop.PropertyType == typeof(System.Drawing.Font))
                {
                    return prevCode + " = " + Property2Font(propertyValue);
                }

                if(prop.PropertyType == typeof(System.Drawing.Color))
                {
                    return prevCode + " = " + Property2Color(propertyValue);
                }

                if(prop.PropertyType == typeof(System.Windows.Forms.Cursor))
                {
                    return prevCode + " = Cursors." + propertyValue;
                }

                string[] split = prop.PropertyType.ToString().Split('.');

                return prevCode + " = " + split[split.Length-1] + "." + propertyValue;

                // resultCode = "-- 未设置[属性]" + propertyName + " - [类型]" + prop.PropertyType.ToString();

            }

            return resultCode;
        }

        private string AnalysisStringItems(string space, string curName, string childName, XElement childElement){

            string itemsCode = space + "self." + curName + "." + childName + ":AddRange(luanet.make_array(String, {";

            foreach(XElement grandsonElement in childElement.Elements())
            {
                itemsCode += "\"" + grandsonElement.Value.ToString() + "\",";
            }

            itemsCode += "}))" + Environment.NewLine;

            return itemsCode;
        }

        private string AnalysisToolStripMenuItem(string space, string curName, string childName, XElement childElement)
        {

            string itemsCode = space + "self." + curName + "." + childName + ":AddRange(luanet.make_array(ToolStripMenuItem, {";

            foreach(XElement grandsonElement in childElement.Elements())
            {
                // grandsonElement 是Item0、Item1这些，还需要继续拿下一个
                XElement itemElement = grandsonElement.Element("Object");
                String itemName = itemElement.Attribute("name").Value.ToString();
                itemsCode += "self." + itemName + ",";
            }

            itemsCode += "}))" + Environment.NewLine;

            return itemsCode;
        }

        private string AnalysisColumnHeader(string space, string curName, string childName, XElement childElement)
        {

            string itemsCode = space + "self." + curName + "." + childName + ":AddRange(luanet.make_array(ColumnHeader, {";

            foreach(XElement grandsonElement in childElement.Elements())
            {
                // grandsonElement 是Item0、Item1这些，还需要继续拿下一个
                XElement itemElement = grandsonElement.Element("Object");
                String itemName = itemElement.Attribute("name").Value.ToString();
                itemsCode += "self." + itemName + ",";
            }

            itemsCode += "}))" + Environment.NewLine;

            return itemsCode;
        }

        private string AnalysisFlatAppearance(string space, string curName, string childName, XElement childElement)
        {
            string itemsCode = "";

            XElement objectElement = childElement.Element("Object");

            foreach(XElement grandsonElement in objectElement.Elements())
            {
                string grandsonName = grandsonElement.Name.ToString();

                itemsCode += space + "self." + curName + ".FlatAppearance." + grandsonName + " = ";

                if(grandsonName.IndexOf("Color") > -1)
                {
                    itemsCode += Property2Color(grandsonElement.Value.ToString());
                }else{
                    itemsCode += grandsonElement.Value.ToString();
                }

                itemsCode += Environment.NewLine;
            }

            return itemsCode;
        }

        

        private string AnalysisListViewItem(string space, string curName, string childName, XElement childElement)
        {

            string itemsCode = space + "self." + curName + "." + childName + ":AddRange(luanet.make_array(ListViewItem, {";

            foreach(XElement grandsonElement in childElement.Elements())
            {
                // grandsonElement 是Item0、Item1这些，还需要继续拿下一个
                string grandsonName = grandsonElement.Name.ToString();

                itemsCode += "self." + curName + grandsonName + ",";
            }

            itemsCode += "}))" + Environment.NewLine;

            return itemsCode;
        }

        private string AnalysisItemsObject(string itemsName, string itemsType, XElement curElement)
        {
            string itemsCode = "";
            foreach(XElement childElement in curElement.Elements(itemsName))
            {

                String childType = childElement.Attribute("type").Value.ToString();

                if(childType.IndexOf(itemsType) > -1){

                    foreach(XElement grandsonElement in childElement.Elements())
                    {
                        // grandsonElement 是Item0、Item1这些，还需要继续拿下一个
                        XElement itemElement = grandsonElement.Element("Object");
                        itemsCode += this.TraversalXmlToLua(itemElement, false);
                    }
                }
            }

            return itemsCode;
        }

        private string AnalysisListViewItemParams(XElement curElement, string curName, string space)
        {
            string code = "";
            foreach(XElement childElement in curElement.Elements())
            {
                // childElement是Item0、Item1这些，还需要继续拿下一个
                code += space + "self." + curName + childElement.Name.ToString() + " = ListViewItem(";
                // 每行只有一个值
                if(childElement.Elements("Param").Count() == 1 ){

                    code += "luanet.make_array(String, {\"" + childElement.Element("Param").Value.ToString() + "\"}))";

                }else{

                    // 有多个Param
                    int index = 0;

                    code += "luanet.make_array(String";

                    foreach(XElement paramElement in childElement.Elements("Param")){

                        index += 1;

                        String paramType = "";

                        if(paramElement.Attributes("type").Count() > 0){
                            paramType = paramElement.Attribute("type").Value.ToString();
                        }

                        if(paramType.IndexOf("System.String") > -1){

                            code += ", {";

                            // 第一个参数，内含多个文本

                            foreach(XElement childParamElement in paramElement.Elements()){

                                code += "\"" + childParamElement.Value.ToString() + "\",";

                            }

                            code += "})";

                        }else{
                            // 其他参数
                            string paramValue = paramElement.Value.ToString();

                            // 颜色参数
                            if(index == 3 || index == 4){
                                if(paramValue == ""){
                                    code += ", Color.Empty";
                                }else{
                                    code += ", " + Property2Color(paramValue);
                                }
                            }else if(index == 5){
                                // 字体参数
                                code += ", " + Property2Font(paramValue);
                            }else{
                                // 剩下的其实就是第二个参数了
                                code += ", " + paramValue;
                            }

                        }
                    }

                    code += ")";
                }

                code += Environment.NewLine;
            }

            return code;
        }

        private string AnalysisItemsParams(string itemsName, string itemsType, XElement curElement, string curName, string space)
        {
            string itemsCode = "";

            foreach(XElement childElement in curElement.Elements(itemsName))
            {

                String childType = childElement.Attribute("type").Value.ToString();

                if(childType.IndexOf(itemsType) > -1){

                    if( itemsType == "ListViewItem"){
                        itemsCode += this.AnalysisListViewItemParams(childElement,curName,space);
                    }

                }
            }

            return itemsCode += Environment.NewLine;
        }


        private string TraversalXmlToLua(XElement curElement, Boolean isRoot)
        {
            string space = "    ";
            // 当前控件的属性代码
            string propertyCode = "";
            // 添加子控件代码
            string addControlCode = "";
            // 添加事件代码
            string addEventCode = "";
            // 添加子项代码
            string addItemsCode = "";
            // 子控件产生的代码
            string childCode = "";

            // 当前控件名称
            String curName = "";

            // 尝试获取控件名称
            if(curElement.Element("Name") != null){
                curName = curElement.Element("Name").Value.ToString();
            }else{
                if(curElement.Attribute("name") != null){
                    curName = curElement.Attribute("name").Value.ToString();
                }
            }

            // 如果该控件没有指定名称，直接跳过
            if(curName == ""){
                return "";
            }

            // 当前控件的类型
            String curType = curElement.Attribute("type").Value.ToString();

            if( isRoot == true ){
                curType = "System.Windows.Forms.Form, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
            }

            // 添加实例化代码
            string[] split1 = curType.Split(',');
            string[] split2 = split1[0].Split('.');
            propertyCode += space + "self." + curName + " = " + split2[split2.Length - 1].Trim() + "()" + Environment.NewLine;

            // 当前控件的属性信息
            PropertyInfo[] curProps = Type.GetType(curType).GetProperties();

            // 获取其他属性
            foreach(XElement childElement in curElement.Elements())
            {
                string childName = childElement.Name.ToString();

                String childType = "";
                if(childElement.Attribute("type") != null){
                    childType = childElement.Attribute("type").Value.ToString();
                }

                // 子控件
                if(childName == "Object")
                {
                    // 过滤设计时控件
                    addControlCode += space + "self." + curName + ".Controls:Add(self." + childElement.Element("Name").Value.ToString() + ")" + Environment.NewLine;
                    continue;
                }

                // 事件
                if(childName == "Event")
                {
                    string event_name = childElement.Attribute("event_name").Value.ToString();
                    string event_text = childElement.Value.ToString();
                    addEventCode += space + "self." + curName + "." + event_name + ":Add(function(sender, e) self:" + event_text + "(sender, e) end)" + Environment.NewLine;

                    if(this.curStartCode.IndexOf("function _M:" + event_text + "(sender, e)") < 0)
                    {
                        // System.Diagnostics.Debug.WriteLine("没找到事件定义");
                        this.curStartCode += "function _M:" + event_text + "(sender, e)" + Environment.NewLine + space + "-- 在这里处理事件" + Environment.NewLine + "end"  + Environment.NewLine + Environment.NewLine;
                    }
                    continue;
                }

                // 子项
                if(childName == "Items" || childName == "DropDownItems")
                {
                    // 处理纯字符串的控件items
                    if(childType.IndexOf("System.String") > -1){
                        addItemsCode += this.AnalysisStringItems(space,curName,childName,childElement);
                    }

                    // 处理ToolStripMenuItem类型
                    if(childType.IndexOf("System.Windows.Forms.ToolStripMenuItem") > -1){
                        addItemsCode += this.AnalysisToolStripMenuItem(space,curName,childName,childElement);
                    }

                    // 处理ListViewItem类型
                    if(childType.IndexOf("System.Windows.Forms.ListViewItem") > -1){
                        addItemsCode += this.AnalysisListViewItem(space,curName,childName,childElement);
                    }

                    continue;
                }

                if(childName == "Columns")
                {
                    // 处理ColumnHeader类型
                    if(childType.IndexOf("System.Windows.Forms.ColumnHeader") > -1){
                        addItemsCode += this.AnalysisColumnHeader(space,curName,childName,childElement);
                    }

                    continue;
                }

                if(childName == "FlatAppearance")
                {
                    propertyCode += this.AnalysisFlatAppearance(space,curName,childName,childElement);

                    continue;
                }

                // 其他属性
                propertyCode += space + this.AnalysisProperty(childElement,curName,curProps) + Environment.NewLine;

            }

            // 再处理子控件
            if( curElement.Elements("Object").Count() >= 1){
                foreach(XElement childElement in curElement.Elements("Object"))
                {
                    childCode += this.TraversalXmlToLua(childElement, false);
                }
            }

            // 
            if( curElement.Elements("Items").Count() >= 1){

                // 一级菜单
                childCode += this.AnalysisItemsObject("Items","ToolStripMenuItem",curElement);

                // ListView具体行
                childCode += this.AnalysisItemsParams("Items","ListViewItem",curElement,curName,space);
            }

            // 二级以下菜单
            if( curElement.Elements("DropDownItems").Count() >= 1){

                childCode += this.AnalysisItemsObject("DropDownItems","ToolStripMenuItem",curElement);
            }

            // ListView表头
            if( curElement.Elements("Columns").Count() >= 1){

                childCode += this.AnalysisItemsObject("Columns","ColumnHeader",curElement);
            }

            return childCode + propertyCode + addItemsCode + addControlCode + addEventCode + Environment.NewLine;
        }

        public string ToLua(string xml, string prevCode)
        {

            if(prevCode == "" || prevCode == null){
                this.curStartCode = this.startCode;
            }else{
                this.curStartCode = prevCode;
            }

            XElement rootElement = XElement.Parse(xml);

            // 判断窗口是否为根节点
            // 当前控件的类型
            if(rootElement.Attribute("type") == null){

                rootElement = (XElement)rootElement.FirstNode;
            }

            String curCode = this.TraversalXmlToLua(rootElement,true);

            String curName = rootElement.Element("Name").Value.ToString();

            string endCode = this.endCode.Replace("Form1",curName);

            return this.curStartCode + this.midCode + curCode + endCode;
        }
    }
}
