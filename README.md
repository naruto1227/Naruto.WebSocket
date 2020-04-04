# Naruto.WebSocket

#### 介绍
此项目是使用中间件对WebSocket 进行封装，方便使用
#### 软件架构
软件架构采用的是.Net Core 3.1


#### 使用说明
1.核心对象<b>NarutoWebSocketService</b>，处理接收服务的操作,使用者需要继承此对象，实现自己的方法，并且方法的返回值必须为Task,并且原生支持DI，生命周期为作用域Scope模式


#### 参与贡献

1.  Fork 本仓库
2.  新建 Feat_xxx 分支
3.  提交代码
4.  新建 Pull Request