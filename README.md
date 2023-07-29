# Charamaker2

これはC#で2Dゲームを作成する際に便利なクラスとか命令とかのセットです。<br>
GameSet1も含めるとゲームエンジンといっても過言ではないでしょう。

[できること]<br>
・bmp画像の表示、回転、透明化<br>
・文字の表示、回転、透明化、フェード。フォントは固定。<br>
・bmp画像の集合体Characterとそれを動かすmotionの作成<br>
・Shapeを用いたあたり判定の作成<br>
・その他Entityやシーンなどのゲームエンジン要素

# License
 
"Charamaker2" is under [MIT license](https://en.wikipedia.org/wiki/MIT_License).
# Features
C#でゲームと言えばUnityですがあちらは複雑すぎて理解ができなかった経験があります。<br>
(今はわかるようになっていますが)<br>
そんな私が作ったものなので、私と波長が合う人はわかりやすいと思います。<br>
ゲーム制作がほとんどソースコードで完結するのもGoodだと思います。

# Requirement
Vortice 各種
https://github.com/amerkoleci/Vortice.Windows
Microsoft.CodeAnalysys 各種
SharpGen.Runtime +.COM
System.Runtime.CompilerServices.Unsafe 


# Build

Charamaker2.slnを開きます。そして開始を押すなりCtrl＋Bなりでビルドをします。その時におそらくエラーが出るでしょう。それを解消すればよいのです。<br>
コンポーネントのインストールは割と自動でやってくれるようです。<br>
コンポーネントのインストールが自動で行われない場合、Vorticeやそれに伴ってSharpGen.RuntimeなどをNugetでインストールする必要が出てくるかもしれません。表示されるエラーに従ってそれらをインストールしてください。<br>

正しくビルドしても、X.resxが開けませんみたいなエラーが出ることがあります。その場合エクスプローラーからファイルXのプロパティをいじります。そのファイルXのプロパティから全般の属性の下にあるWebなんたらを許可するにチェックを入れて適用します。<br>
そしたらビルドが成功し、実行したら青い変な画面が出るはずです。<br>

ビルドファイルの中身がだいたいこんな感じ.pngみたいになったら成功です。<br>

# どんなゲームが作れるか

With Ball Dribbleのゲーム作品をご覧ください。<br>すべて2Dで、アクションRPG、カードゲーム、ピンボールなどです。

# Usage

基本はWindowsFormで行います。<br>
だいたいはサンプルに描いてるのがすべてなのでSampleを見てください<br>
filemanのセッティングアップからhyojimanをmakeして.inputinをMouseDownとかに接続します<br>
あとはお好みでEntityとかSceneとかを作ればいいと思います。<br>
ビルドした中身を自分のプログラムのReleaseにコピーします。<br>
Charamaker2.exeをソリューションエクスプローラーから参照に追加し、更に同梱されてるSystem.Runtime.CompilerServices.Unsafe.dllも参照に追加すればビルドができるようになると思います<br>
また含まれているリソースは全て著作権は私のものです。再配布さえしなければ煮るなり焼くなりどうぞ。

# エラーとか
～～の参照を解決できませんみたいなエラーは実行ファイルの隣にdllか何かが足りない時に発生するはずです。どこかからそのdllを探して実行ファイル横にぶち込んでおきましょう。<br>
filemanを使用する際にSystem.Numericsが解決できませんみたいなエラーが発生することがあります。そうすると出力のところにapp.configをいじってくださいみたいなメッセージが出るのでそれに従っていじると直ります。

# AboutTemplate

基本的なEntityとSceneのクラスを構えているGameSet1を作りました。<br>
それに際してTemplateを用意しました。使えるようにするには
<br>
\Visual Studio 2022\Templates\ProjectTemplates
<br>
にGameSet1Project.zipを置いてください。<br>
それをもとに新規プロジェクトを作成すると、パスを通したインストールとかしてないのでCharamakaer2,Gameset1の参照がおかしなことになってると思います。なのでResource内の同名のファイルかあなたがビルドしたCharamaker2に参照を変更してあげてください<br>
これでテンプレートをビルドしようとすると「Vorticeがないよ」みたいなエラーが起きる場合があります。<br>
その場合はCharamaler2.slnを開き、一度ビルドできるまで頑張ってください。そしたら、
Charamaker2\GameSet1\bin\Debug\にCharamaker2.exe,GameSet1.dllができているのでそれを参照し直してください。<br>
このCharamaker2.slnのビルドができない場合はBuildをご覧ください。<br>

Resourceの中にリソースフォルダ群を置いといてあるのでそちらも.exeの横においてご活用ください。


# About通信
P2P通信を可能にするやつを作りました。仕様上サーバーからクライアントにhyojimanをクライアントがサーバーにinputinを送るだけですが割と何とかなると思います。<br>
実行した際に、mrwebrtc.dllが見つかりませんというエラーが起こることがあります。その場合packages\Microsoft.MixedReality.WebRTC.2.0.2\runtimes\win10-x86\nativeにあるそいつを.exeファイル下に持っていきましょう。<br>
通信はIPなど含まれたメッセージを何かしらの手段で相手に伝え、相手がそれを入力、つまり人力シェイクハンドで行います。なので通信するためにはSTUNサーバー以外必要ありません。<br>
こちらの機能はMicrosoft.MixedReality.WebRTCを使用しております。<br>




