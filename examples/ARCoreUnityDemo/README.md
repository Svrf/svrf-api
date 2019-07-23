# ARCore Face Filters using the Svrf API

This example uses the [Svrf API][Docs] to create a Face Filter Unity App using [ARCore][]. In this example, users can search for [Svrf][] Face Filters and experience them.

## About ARCore

[Google ARCore][ARCore] provides real-time information about the pose of the user's face. See the [devices][ARCore Support] that support ARCore.

## Svrf + ARCore

Using ARCore's face detection and the Svrf API, you can apply 3D face filters to a user's face. Svrf's 3D face filters are streamed to the device at runtime giving your users access to the entire Svrf library of face filters.

## Requirements

- [Unity][] 2017.4.26f1 or later.
- Android SDK 7.0 (API Level 24) or later.
- An ARCore [supported][ARCore Support] Android phone.

## Get Started

Clone the repository and navigate to the example.

```bash
git clone https://github.com/Svrf/svrf-api.git && cd ./svrf-api/examples/ARCoreUnityDemo
```

Open the `ARCoreUnityDemo` folder as Unity project.

Add you Svrf API key to the `Svrf API Key` game object. You can learn more about acquiring an API key at [developers.svrf.com][Docs].

[ARCore]: https://developers.google.com/ar/develop/unity/quickstart-android
[ARCore Support]: https://developers.google.com/ar/discover/supported-devices
[Docs]: https://developers.svrf.com
[SVRF]: https://www.svrf.com
[Unity]: https://unity3d.com/get-unity/download
