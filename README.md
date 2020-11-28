# Coco.Comsumer

Coco是一个使用dotnet来实现的简单高效消息队列，本项目是Coco消息队列的asp.net core消费者插件.

![GitHub Workflow Status](https://img.shields.io/github/workflow/status/sbchong/Coco.Comsumer/Coco.Comsumer)
![Nuget](https://img.shields.io/nuget/dt/Coco.Comsumer)
![GitHub](https://img.shields.io/github/license/sbchong/Coco.Comsumer)

## 使用方法

引入nuget包

```bash
$  dotnet add package Coco.Comsumer
```

建立新文件 ，内容如下，SubScribe 方法内容即为消息收到处理过程，这里使用logger打印订阅消息

```C#
public class MyComsumerService : ComsumerService
{
    private readonly ILogger<MyComsumerService> _logger;
    public MyComsumerService(ILogger<MyComsumerService> logger, string host = "192.168.2.10", string topicName = "message") : base(host, topicName)
    {
        _logger = logger;
    }

    protected override void SubScribe(string message)
    {
        if (!string.IsNullOrEmpty(msg))
        {
            _logger.LogInformation(msg);
        }
    }
}
```

服务注册，使用泛型注册，传入自定义服务类

```C#
public void ConfigureServices(IServiceCollection services)
{
     services.AddControllers();

     services.AddCocoComsumer<MyComsumerService>();
}
```

**这里为了方便说明使用了新文件，开发时亦可以不建立新文件，只需要继承ComsumerService，并实现SubScribe方法即可**
