ScanSnapHelper
==============

ScanSnap Manager および ScanSnap Manager for fi Series の補助ツールです。

ScanSnap Manager（for fi Series）がスキャンの過程で RAW 画像を利用する直前で、ScanSnapHelper.exe と同階層にある hook.bat を呼び出します。
hook.bat には RAW 画像のパスが渡されますので、hook.bat からスクリプトを起動する等して

* RAW 画像をコピーして取っておく
* RAW 画像を別のものに差し換える

等の処理が行えます。

動作環境
--------

.NET Framework 3.5 が必要です。

インストール
------------

.zip を適当なパスに展開してください。
UAC が有効な環境では、%PROGRAMFILES% 配下でないほうが、編集の際に便利かもしれません。

使用方法
--------

ScanSnapHelper.exe を起動すると、勝手に

* ScanSnap Manager（PfuSsMon.exe）
* ScanSnap Manager for fi Series（PfuSsMff.exe）

の処理をフックします。

正常にフックされた場合は「Hooked: 【プロセスID】」と表示されます。
「Not hooked:」と表示される場合は ScanSnap Manager（for fi Series）が起動していないのだと思いますので、起動してください。

フック状態でスキャンを行うと、hook.bat が起動されます。

サンプルとして、3 つのスクリプトを含めてあります。

* copy_here.py
  * RAW 画像をスクリプトと同階層にコピーします。
  * ScanSnap Manager では ScanSnap0.raw、ScanSnap1.raw のような名称になります。
  * 継続読み取りの際は連番は 0 に戻ってしまいますので、長期保存には向きません。
* replace_images.py
  * RAW画像をスクリプトと同階層にある同名のファイルで置き換えます。
  * copy_here.py で保存した RAW 画像を再利用し、ScanSnap Manager の設定によって結果がどう変わるかを検証するのに使えます。
* rotate_for_comic.py
  * 右綴じ・右開きの本を横向きにスキャン可能にするためのものです。
    * 実際にやってみるとクロッピングがおかしなことになったので、実用には向きません。
  * [JPEG Lossless Rotator](http://annystudio.com/software/jpeglosslessrotator/) を .\jpegr_portable64 として配置した上で使う必要があります。
  * 偶数ページは左に 90 度、奇数ページは右に 90 度回転させます。

全て Python で書かれています。よくわからない方はとりあえず ActivePython 2.7 系の x86 を入れておけば動かせます。

配布時の hook.bat は、copy_here.py を呼び出す処理が書かれています。
他の 2 つは先頭に「::」を付加してコメントアウトしている状態ですので、必要に応じてコメントアウト／アンコメントして使ってください。
ログは hook.log に出力していますので、スクリプトがうまく動作しない場合は見てみてください。

ビルド方法
----------

[Microsoft Visual Studio Express 2012 for Windows Desktop](http://www.microsoft.com/ja-jp/download/details.aspx?id=34673) で作っていますので、それ以降のバージョンでビルドしてください。

利用ライブラリ
--------------

[EasyHook](http://easyhook.codeplex.com/) を使っています。
