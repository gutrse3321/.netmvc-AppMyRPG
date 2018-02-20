# .netmvc-AppMyRPG
My Old .NET MVC Project 以前做过的一个.NET MVC项目，个人全栈
## 小碎念
<del>我还是想直接做个黄色游戏网...</del>
# 开发环境
+ 系统：Windows 10,
+ IDE：Visual Studio 2017
+ .NET Framework 4.5,EntityFramework 6.0
# 关注目录
+ App_Data(LocalDB)
+ Areas/Admin
+ Controllers
+ Models
+ Views
# 前台展示
### 首页
![首页](https://i.loli.net/2018/02/20/5a8c31768573f.jpg)
### 游戏页
![游戏页](https://i.loli.net/2018/02/20/5a8c31760e1ef.jpg)
### 登陆后
![登陆后](https://i.loli.net/2018/02/20/5a8c336b4aa06.jpg)
# 后台展示
### 首页
![首页](https://i.loli.net/2018/02/20/5a8c3176a0b5d.png)
### 游戏列表
![游戏列表](https://i.loli.net/2018/02/20/5a8c3176d37a4.png)
# 数据库相关
### 将LocalDB替换SQL Server
```xml
<!-- ./AppMyRPG/Web.config -->
<connectionStrings>
    <add name="DB_MyRPGEntities" 
			  connectionString="Data Source=.;
			  Initial Catalog=DB_MyRPGEntities;
 			 user=sa;password=*******;
			  MultipleActiveResultSets=True;
			  App=EntityFramework" 
			  providerName="System.Data.EntityClient" />
</connectionStrings>
<entityFramework>
    <defaultConnectionFactory 
		type="System.Data.Entity.Infrastructure.LocalDbConnectionFactory,
		EntityFramework">
    </defaultConnectionFactory>
    <providers>
      <provider invariantName="System.Data.SqlClient" 
	  				 type="System.Data.Entity.SqlServer.SqlProviderServices,
					   EntityFramework.SqlServer" />
    </providers>
</entityFramework>
```
