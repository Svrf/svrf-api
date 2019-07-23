# ARKit Face Filters using the Svrf API

This example uses the [Svrf API][Docs] to create a Face Filter iOS App using [ARKit][]. In this example, users can search for [Svrf][] Face Filters and experience them using their front-facing TrueDepth camera.

![ARKit Face Filter SDK Search Demo](./assets/face-filter-search-demo.jpeg) ![ARKit Face Filter SDK Demo](./assets/face-filter-demo.gif)

## About ARKit

[ARKit][] on iPhone X and newer uses the front-facing TrueDepth camera to provide real-time information about the pose and expression of the user's face.

## Svrf + ARKit

Using ARKit's face detection and the Svrf API, you can apply 3D face filters to a user's face that react to their facial expressions in real-time. Svrf's 3D face filters are streamed to the device at runtime giving your users access to the entire Svrf library of face filters.

## Requirements

- [Xcode 10][Xcode]
- [CocoaPods][]
- iPhone X or newer with iOS 11+

## Get Started

Clone the repository and navigate to the example.

```bash
git clone https://github.com/Svrf/svrf-api.git && cd ./svrf-api/examples/ARKitFaceFilterDemo
```

Install the dependencies using [CocoaPods][].

```bash
pod install
```

Open `ARKitFaceFilterDemo.xcworkspace` in Xcode.

Configure [`./ARKitFaceFilterDemo/Plists/Info.plist`][Plist] with your **SVRF_API_KEY**. You can learn more about acquiring an API key at [developers.svrf.com][Docs].

```plist
<plist version="1.0">
  <dict>
    <key>SVRF_API_KEY</key>
    <string>{your-api-key}</string>
    <!-- ... -->
  </dict>
</plist>
```

To build and test the app, connect an iPhone X or newer and run the app.

[ARKit]: https://developer.apple.com/arkit/
[CocoaPods]: https://cocoapods.org/
[Docs]: https://developers.svrf.com
[Plist]: ./ARKitFaceFilterDemo/Plists/Info.plist
[Svrf]: https://www.svrf.com
[Xcode]: https://developer.apple.com/xcode/
