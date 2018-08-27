# 基本架构设计
## 客户端架构
![main](https://github.com/amcp9/ImageCache/blob/master/Shooter/facade.png)
## 服务器架构
#### a
#### a
## 通信模式
#### 使用Socket(TCP)服务完成C/S数据通信
### 数据格式
![main](https://github.com/amcp9/ImageCache/blob/master/Shooter/dataformat.png)
#### ·客户端向服务器发送的数据，服务器通过RequestCode找到对应的控制器处理，再通过ActionCode通过C#反射机制找到对应的函数处理
#### ·服务器向客户端发送的数据，客户端通过ActionCode找到对应的脚本进行处理
#### ·数据添加数据长度首部,处理TCP粘包与分包
## 通信架构
![main](https://github.com/amcp9/ImageCache/blob/master/Shooter/SendManager.png)
## 对象池应用
#### 1.红蓝方弓箭
#### 2.红蓝方弓箭爆炸效果
# IP设置
#### 服务器Server设置位于Programer.cs
#### MYSQL数据库设置位于Tools/ConnectionHelper.cs
#### Client设置位于Scripts/Net/ClientManager.cs
