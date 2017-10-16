using System;
using WeihanLi.EntityFramework.DbDescriptionHelper;

namespace PowerControlDemo.Models
{
    /// <summary>
    /// 权限访问设置表
    /// </summary>
    [TableDescription("ShopAccessConfig", "权限访问设置表")]
    public class ShopAccessConfigModel : BaseModel
    {
        [ColumnDescription("主键")]
        public new long PKID { get; set; }

        private string menuName;

        [ColumnDescription("菜单名称")]
        public string MenuName
        {
            get { return menuName; }
            set { menuName = value; }
        }

        private string elementSelector;

        [ColumnDescription("元素选择器")]
        public string ElementSelector
        {
            get { return elementSelector; }
            set { elementSelector = value; }
        }

        private int parentId;

        [ColumnDescription("父级id")]
        public int ParentId
        {
            get { return parentId; }
            set { parentId = value; }
        }

        private string areaName;

        [ColumnDescription("Area名称")]
        public string AreaName
        {
            get { return areaName; }
            set { areaName = value; }
        }

        private string controllerName;

        [ColumnDescription("Controller名称")]
        public string ControllerName
        {
            get { return controllerName; }
            set { controllerName = value; }
        }

        private string actionName;

        [ColumnDescription("Action名称")]
        public string ActionName
        {
            get { return actionName; }
            set { actionName = value; }
        }

        private int controlType;

        [ColumnDescription("控制类型,0:页面View,1:页面元素Element")]
        public int ControlType
        {
            get { return controlType; }
            set { controlType = value; }
        }

        private int displayType;

        [ColumnDescription("显示类型,0:显示,1:不显示,2:显示但禁用")]
        public int DisplayType
        {
            get { return displayType; }
            set { displayType = value; }
        }

        private Guid accessKey;

        [ColumnDescription("权限访问Key")]
        public Guid AccessKey
        {
            get { return accessKey; }
            set { accessKey = value; }
        }

        private int shopType;

        [ColumnDescription("允许的门店类型")]
        public int ShopType
        {
            get { return shopType; }
            set { shopType = value; }
        }
    }
}