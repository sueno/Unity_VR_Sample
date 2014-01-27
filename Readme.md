# Unity virtual reality Sample

## 紹介動画など

https://www.youtube.com/user/javajunkies409

## 概要

UnityとKinect,iPhone,HMD(Oculus rift)等を用いてVR体験をするプロジェクトです。  
  
- 使用するデバイス
 + Kinect	（必須）
 + iPhone	（任意、AppleDeveloperライセンスがあれば、RemoteController_for_Unityリポジトリからコントローラーのプログラムを使用できます）
 + Android	（任意、そのうちアプリをストアに上げます）
 + Oculus rift	（任意、そのうち本プロジェクト用にカスタマイズしたassetを公開します）

- ゲームを想定した機能も多少用意しています  
 + 生き物オブジェクト	：　HP等いくつかのステータスを持ちます
 + 武器オブジェクト	：　生き物オブジェクトにダメージを与えます
 + アイテムオブジェクト	：　手で掴んだりできます
 + その他今後実装予定...
  
- その他
 + MMDのモデル（pmx限定）を簡単に読み込む機能

## 動作環境

Windows7,8 なら多分大丈夫  
  
Unity4.3.2  
  
## インストール手順

注意！
本プロジェクトは十分な動作確認を行っていないため、
正常に動作するケースは稀です。  
8割型失敗しても良い方のみお試しください。  
詳細な手順はそのうち書きます。  

0. 必要なもの
 + Kinect for Windows SDK 1.8	（必須）
 + ZDK(Zigfu)		 （必須）
 + Unity4.3.2		 （必須、Unity4.3以降であれば動作するはずです）
 + mmd-for-unity	 （MMDのモデルを動かしたい人向け）
 + LitJSON		 （iPhone,Androidのコントローラを使う場合必要）
 + websocket-sharp	 （マルチプレイしたい場合必要、サーバ側プログラムは今後公開予定）
 + Osulus rift developper tool kit ? (Oculus rift 使いたい人)

1. Microsoftから、Kinect for Windows SDK 1.8 のインストール

2. Unity4.3.2の入手（Unity4.3以降）

3. Zigfuから、ZDKのダウンロード
 + ここで、Kinectのサンプルプロジェクトの動作確認をしてみてください。

4. 本プロジェクト(Unity_VR_Sample)の入手
  
以降、必要な場合のみ  
  
5. LitJSON,websocket-sharpのダウンロード

6. Oculus rift 使いたい人は適当に入れて下さい

## 使用方法（mmd-for-unityを使用する場合）

1. Unityのプロジェクトを作成します

2. Assetsフォルダに、ZDK,Unity_VR_Sample,mmd-for-unity(,LitJSON,websocket-sharp,OVR)を入れます

3. Assetsフォルダに、Kinectで動かしたいモデル（以後モデル）を入れます

4. モデルのpmxファイルを選択すると、Inspector（ウィンドウ右側）にmmd-for-unityのconvertなんちゃらが出てくるので、`AnimationType`を`Human Mecanim`にして`Convert Prefab`をおします。モデルがSceneに追加されれば成功です。

4. `Unity_VR_Sample/Prefab/MainPlayer.prefab`をHierarchy（ウィンドウ左上）にドラッグ＆ドロップします

5. Hierarchyにあるモデルを先ほどHierarchyに追加したMainPlayer内にドラッグ＆ドロップします

6. Hierarchyのモデルに`Unity_VR_Sample/MainPlayer/MainCharactorController.cs`、`Unity_VR_Sample/UseAnothoerLibraryClasses/Zigfu/CustomZigSkeleton.cs`を追加（ドラッグ＆ドロップ）します。

7. Hierarchyのモデルを選択し、Inspectorの`Main Charactor Controller`の項目を開き、次のように設定します
 + `Main Camera`の項目に、Hierarchyの`MainPlayer/cameraposition`をドラッグ＆ドロップします
 + `Data`の中の`RootObject`にHierarchyの`MainPlayer`をドラッグ＆ドロップします
 + `Main Player`にチェックを入れます
 + iPhone,Androidコントローラを使用する場合、`Use Remote Controller`にチェックを入れます

8. Hierarchyの`MainPlayer/ZigFu`を選択し、Inspectorの`Zig Engage Single User`の項目を開き、次のように設定します
 + `Engaged Users`の中の`Element 0`にHierarchyのモデルをドラッグ＆ドロップします

9. 使っていないライブラリのクラスを消します（`Unity_VR_Sample/UseAnothoerLibraryClasses/websocket-sharp`,`Unity_VR_Sample/UseAnothoerLibraryClasses/LitJSON`あたり）

10. 実行します

（11. エラーが出るので、バグを報告します

## 使用方法（自作、またはUnityのモデルを使用する場合）

頑張る。