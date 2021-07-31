# HoloLensJapanCOVID19Viewer
- HoloLens 2 でコロナワクチン接種状況を可視化するアプリ

# 利用データ
## ワクチン接種データ
- 新型コロナワクチンの接種状況（一般接種（高齢者含む））
- [https://cio.go.jp/c19vaccine_dashboard](https://cio.go.jp/c19vaccine_dashboard)
- GET https://vrs-data.cio.go.jp/vaccination/opendata/latest/prefecture.ndjson --output data.gzip

## 人口データ
- 【総計】令和2年住民基本台帳年齢階級別人口（市区町村別）
- [https://www.soumu.go.jp/main_content/000701583.xls](https://www.soumu.go.jp/main_content/000701583.xls)

## 日本地形データ
- 国土地理院ウェブサイト(加工済み)
- [https://maps.gsi.go.jp/3d/](https://maps.gsi.go.jp/3d/)

## 利用ライブラリ
- Mixed Reality Toolkit
- [https://github.com/microsoft/MixedRealityToolkit-Unity](https://github.com/microsoft/MixedRealityToolkit-Unity)
