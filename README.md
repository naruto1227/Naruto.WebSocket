# Naruto.WebSocket

#### 介绍
此项目是使用中间件对WebSocket 进行封装，方便使用
<br>单机10000连接，内存占用220M
#### 软件架构
软件架构采用的是.Net Core 3.1
#### 支持
1. 支持点对点的发送消息，支持群聊消息，支持发送所有人的消息，支持发送指定用户消息，支持集群扩展,支持多租户的模式.
2. 支持授权验证
3. 用户可以使用<b>NarutoWebSocketEvent</b>处理上下线的事件通知

#### 使用说明
1. 核心对象<b>NarutoWebSocketService</b>，处理接收服务的操作,使用者需要继承此对象，实现自己的方法，且方法的访问级别为Public,方法的参数支持无参和实例对象两种，并且方法的返回值必须为Task,并且原生支持DI，生命周期为作用域Scope模式
2. 示例 安装Nuget包<b>Naruto.WebSocket</b>, 并注入所需服务和对应的接收服务的对象
```c#
            //注入服务
            services.AddNarutoWebSocket<MyService>(a =>
            {
                a.Path = new PathString("/ws");//websocket的请求路径
                a.AuthorizationFilters.Add(new MyAuthorizationFilters());//追加websocket连接的授权信息
            });
```
3. 使用<b>IClientSend\<TService></b>可以直接从外围操作消息的发送
4. 集群版,安装Nuget包<b>Naruto.WebSocket.Redis</b>,然后还需注入
```c#
            //注入集群版需要的服务
            services.AddNarutoWebSocketRedis(a => a.Connection = new string[] { "127.0.0.1:6379" });


```
5. 客户端使用当客户端发送websocket消息的时候，数据格式为json格式数据，并且必须包含一个action的key，当前action指定的是当前发送的消息，指派给后端实现了<b>NarutoWebSocketService</b>的对象的对应方法来处理。
``` javascript
    //当前的写法的含义是发送给后端的leave方法处理websocket的发送的消息，参数为一个对象中包含的属性为roomId
     var msg = {
            action: "leave",
            roomId: document.getElementById("txtRoomId").value
        };
        webSocket.send(JSON.stringify(msg));
```
6. 用户可以在创建websocket客户端的时候主动传递一个当前websocket的连接Id，不传递则由后台自动生成
```javascript
    //主动传递一个连接Id的值ConnectionId
     var webSocket = new WebSocket("ws://localhost:5003/ws?ConnectionId=12345678");
```
#### 参与贡献

1.  Fork 本仓库
2.  新建 Feat_xxx 分支
3.  提交代码
4.  新建 Pull Request