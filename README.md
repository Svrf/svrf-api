# Getting Started

The Svrf API allows you to supercharge your project or app with the first and largest search engine for immersive experiences. We make it simple for any developer to incorporate highly immersive experiences with all kinds of applications: virtual reality, augmented reality, mixed reality, mobile, and web.

The Svrf API is based on REST principles, allowing it to integrate cross-platform. Our endpoints return responses in [JSON][]. We support [CORS][], allowing you to access immersive experiences indexed by Svrf on your own web domains. We provide a variety of resolutions, projections, and file formats to support most modern clients.

Svrf API Documentation is available at [https://developers.svrf.com][Svrf Dev].

## Access and API Keys

To  generate a Svrf API Key, create an account on [www.svrf.com][Svrf] and go to the *Apps* section of the [User Settings][Svrf User Settings] page.

See our [terms of service][TOS] for restrictions on using the Svrf API, libraries, and SDKs.

If you have questions or need support, please [create a ticket][Support].

## API Highlights

## Search Endpoint

[The Svrf Search Endpoint][Docs Search] brings the power of immersive search found on [Svrf.com][Svrf] to your app or project. Our search engine enables your users to instantly find the immersive experience they're seeking. Content is sorted by the Svrf rating system, ensuring that the highest quality content and most relevant search results are returned first.

### Trending Endpoint

[The Svrf Trending Endpoint][Docs Trending] provides your app or project with the hottest immersive content - curated by real humans. The experiences returned mirror the [Svrf homepage][Svrf], from timely cultural content to trending pop-culture references. The trending experiences are updated regularly to ensure users always get fresh updates of immersive content.

## API Libraries

You can use Svrf API libraries to encapsulate endpoint requests:

* [C#][CSharp]
* [Java][]
* [JavaScript][]

## SDKs

You can use the Svrf SDKs to encapsulate endpoint requests and render 3D face filters in your application.

* [iOS SDK][]
* [Unity SDK][]

## Attribution

### Authors and Site Credit

At Svrf, we believe in giving credit where credit is due. Do your best to provide attribution to the `authors` and `site` where the content originated. We suggest using the format: __by {authors} via {site}__

If possible, please provide a way for users to discover and visit the page the content originally came from (`url`).

### Powered By Svrf

As per section 5 A of the [terms of service][TOS], __we require all apps that use the Svrf API to conspicuously display "Powered By Svrf" attribution marks where the API is utilized.__

## Rate Limits

The Svrf API has a generous rate limit to ensure the best experience for your users. We rate limit by IP address with a maximum of 100 requests per 10 seconds. If you exceed the rate limit, requests from the requesting IP address will be blocked for 60 seconds.

[API Email]: mailto:api@svrf.com
[CORS]: https://en.wikipedia.org/wiki/Cross-origin_resource_sharing
[CSharp]: https://docs.svrf.com/docs/csharp
[Docs Search]: https://developers.svrf.com/#tag/Media/paths/~1vr~1search?q={q}/get
[Docs Trending]: https://developers.svrf.com/#tag/Media/paths/~1vr~1trending/get
[iOS SDK]: https://docs.svrf.com/docs/ios
[Java]: https://github.com/Svrf/svrf-java-client
[JavaScript]: https://docs.svrf.com/docs/web
[JSON]: http://www.json.org/
[Support]: https://docs.svrf.com/discuss-new
[Svrf]: https://www.svrf.com
[Svrf Dev]: https://developers.svrf.com
[Svrf User Settings]: https://www.svrf.com/user/settings
[Swift]: https://github.com/Svrf/svrf-swift4-client
[TOS]: https://www.svrf.com/terms
[Unity SDK]: https://docs.svrf.com/docs/unity
