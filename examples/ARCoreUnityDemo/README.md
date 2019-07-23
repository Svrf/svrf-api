# ARCore Face Filters using the SVRF API

This example uses the [SVRF API][Docs] to create a Face Filter Unity App using [ARCore][]. In this example, users can search for [SVRF][] Face Filters and experience them.

## About ARCore

[Google ARCore][ARCore] provides real-time information about the pose of the user's face. Check out its [supported devices](https://developers.google.com/ar/discover/supported-devices).

## SVRF + ARCore

Using ARCore's face detection and the SVRF API, you can apply 3D face filters to a user's face. SVRF's 3D face filters are streamed to the device at runtime giving your users access to the entire SVRF library of face filters.

## Requirements

- [Unity][] 2017.4.26f1 or later.
- Android SDK 7.0 (API Level 24) or later.
- An ARCore [supported][supported devices] Android phone.

## Get Started

Clone the repository and navigate to the example.

```bash
git clone https://github.com/SVRF/svrf-api.git && cd ./svrf-api/examples/ARCoreUnityDemo
```

Open `ARCoreUnityDemo` folder as Unity project.

Add you Svrf API key to the `Svrf API Key` game object. You can learn more about acquiring an API key at [developers.svrf.com][Docs].

[ARCore]: https://developers.google.com/ar/develop/unity/quickstart-android
[Docs]: https://developers.svrf.com
[supported devices]: https://developers.google.com/ar/discover/supported-devices
[SVRF]: https://www.svrf.com
[Unity]: https://unity3d.com/get-unity/download
