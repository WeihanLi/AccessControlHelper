using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PowerControlDemo.Models
{
    /// <summary>
    /// 路由信息表
    /// </summary>
    [TableDescription("ShopRouteInfo", "路由信息表")]
    public class ShopRouteInfoModel:BaseModel
    {
        /// <summary>
        /// 路由类型，0：Area，1：Controller，2：Action
        /// </summary>
        [ColumnDescription("路由类型，0：Area，1：Controller，2：Action")]
        public int RouteType { get; set; }

        /// <summary>
        /// 路由信息名称
        /// </summary>
        [ColumnDescription("路由信息名称")]
        public string RouteInfoName { get; set; }

        /// <summary>
        /// 路由信息描述
        /// </summary>
        [ColumnDescription("路由信息描述")]
        public string RouteInfoDesc { get; set; }

        /// <summary>
        /// 父级路由id
        /// </summary>
        [ColumnDescription("父级路由id")]
        public int ParentId { get; set; }
    }
}