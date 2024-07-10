<h1 align="center">
    ☀🌊 Summer 🌊☀
</h1>

<h4 align="center">
   For those who loved Spring, here's what comes next.
</h4>

<p align="center" width="100%">
    <img width="10%" src="https://github.com/ricardoianelli/Summer/actions/workflows/dotnet.yml/badge.svg">
</p>

## ❓ Why? ❓
I used to use Spring on Java and loved how productive I was able to be. Ever since moving back to C#, I have always felt like C# is great, but I do miss some of the things that made Java nice to use.
Because of that, I decided to start this project, mainly as a way of having fun.

## 🚀 Goals 🚀
The initial goal is to make a C# Framework that has:

- Dependency Injection using attributes.
- Async Events in a Publish/Subscribe pattern.
- Something close to AOP, enabling cross-cutting concerns to be defined out of your business logic. (Ex.: Logs, Exception Handling, Metrics, etc)
- Have a ready-to-go dashboard/control panel where you can see metrics, query logs, create alerts, etc.
- Extension methods to make our lives easier. (HttpClient.PostAsync with timeout, etc).
      
**+ New stuff suggested by you to make our lives easier!**

## ✏ Disclamer ✏
I don't mean to create a super high-quality project working now and then after an exhausting day at work, alone. So, in the beginning, I'm not really concerned about security or performance, I'll worry about those things once I get something worth securing and the performance is bad.


This is supposed to be a fun project where I can work and hopefully learn more and make my life easier, and if I'm doing that, why not share it with other people? That way I can learn more and together we can make an even better product that all of us can use. 
So please, if you see something that could be improved or feel like something is not easy to understand, open an issue, and let's talk about it. 

As long as we grow together, this project is already a win.

## 🚧 Progress 🚧
#### Core:
- [x] Dependency Injection using attributes. (Example: [Inject]) **([PR](https://github.com/ricardoianelli/Summer/pull/2) - [Docs](docs/DI.md))**
- [X] Async Events in a Publish/Subscribe pattern. **([PR](https://github.com/ricardoianelli/Summer/pull/3) - [Docs](docs/AsyncEvents.md))**
- [X] Add more functionality to Events (Accept Synchronous, etc). **([PR](https://github.com/ricardoianelli/Summer/pull/4))**
- [X] Simple command queue implementation. **([PR](https://github.com/ricardoianelli/Summer/pull/5))**
- [ ] Aspect-Oriented Programming
- [ ] Metrics Dashboard
- [ ] Logs Dashboard
- [ ] Alerts

#### Enhancements:
- [ ] Add support to .Net Framework (For people using Unity Engine and Legacy Apps)
- [ ] Make it possible to do constructor injection.
- [ ] Make it possible to do DI during runtime.
- [ ] Remove the need of implementing IComponent to be a component.
- [ ] Make it possible to do DI of non-component classes.
- [ ] Add support for non-singleton components and injections.
- [ ] HttpClient extension methods supporting timeout.
