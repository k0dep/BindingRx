# AutoInjector
Auto injector for Zenject package.  

## Using
For start using this package add lines into `./Packages/manifest.json` like next sample:  
```json
{
  "dependencies": {
    "autoinjector": "https://github.com/k0dep/AutoInjector.git#1.0.0"
  }
}
```
Instead `#1.0.0` paste version what you need.

> **Warning!** For unity 2018.3, After include dependency in manifest,
> you must manual add [TypeInspector](https://github.com/k0dep/type-inspector) and [Zenject](https://github.com/k0dep/Zenject) dependencies in manifest