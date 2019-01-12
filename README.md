# Favicon API

Get the favicon for a website either as image or base64 from the website URL.

## Getting Started

- Run Docker image sboulema/faviconapi
- Go to `{{host}}` to test the api using Swagger

## Parameters

- **url**  (string): URL of the website you want to get the favicon from
- **base64** (bool): Favicon is returned as image by default, this switches output to base64 data url

## Favicon retrieval logic

The API will try to fetch the website favicon with the following methods in order.

1. Check `link[contains(@rel, 'apple-touch-icon')]`
2. Check `link[contains(@rel, 'icon')]`
3. Check `favicon.ico`