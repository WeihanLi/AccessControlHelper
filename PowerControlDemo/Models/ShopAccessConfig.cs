using System;

namespace PowerControlDemo.Models
{
    public class ShopAccessConfigModel : BaseModel
    {
        private string menuName;

        public string MenuName
        {
            get { return menuName; }
            set { menuName = value; }
        }

        private int parentId;

        public int ParentId
        {
            get { return parentId; }
            set { parentId = value; }
        }

        private string areaName;

        public string AreaName
        {
            get { return areaName; }
            set { areaName = value; }
        }

        private string controllerName;

        public string ControllerName
        {
            get { return controllerName; }
            set { controllerName = value; }
        }

        private string actionName;

        public string ActionName
        {
            get { return actionName; }
            set { actionName = value; }
        }

        private int controlType;

        public int ControlType
        {
            get { return controlType; }
            set { controlType = value; }
        }

        private int displayType;

        public int DisplayType
        {
            get { return displayType; }
            set { displayType = value; }
        }

        private Guid accessKey;

        public Guid AccessKey
        {
            get { return accessKey; }
            set { accessKey = value; }
        }
    }
}