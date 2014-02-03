using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class CurvesTransferer {
	const string duplicatePostfix = "_";
	
	[MenuItem("Assets/Transfer Clip Curves to Copy")]
	static void CopyCurvesToDuplicate()
	{
		List<AnimationClip> clipList = new List<AnimationClip>();
		clipList.Clear();
		foreach( Object obj in Selection.objects){
			AnimationClip clip = obj as AnimationClip;
			if ( clip != null ) {
				clipList.Add(clip);
			}
		}
		
		if ( clipList.Count == 0 ) {
			return;
		}
		
		foreach( AnimationClip clip in clipList ) {
			AnimationClip copyClip = createFile(clip);
			duplicate( clip, copyClip );
			Debug.Log("Copying curves into " + copyClip.name + " is done");
		}
	}
	static AnimationClip createFile(AnimationClip importedClip)
	{
		string importedPath = AssetDatabase.GetAssetPath( importedClip );
		string copyPath = importedPath.Substring(0, importedPath.LastIndexOf("/"));
		copyPath += "/" + duplicatePostfix + importedClip.name + ".anim";
		
		AnimationClip src = AssetDatabase.LoadAssetAtPath(importedPath, typeof(AnimationClip)) as AnimationClip;
		AnimationClip newClip = new AnimationClip();
		newClip.name = duplicatePostfix + src.name;
		AssetDatabase.CreateAsset(newClip, copyPath);
		AssetDatabase.Refresh();
		return newClip;
	}
	static AnimationClip duplicate(AnimationClip src, AnimationClip dest)
	{
		AnimationClipCurveData[] curveDatas = AnimationUtility.GetAllCurves(src, true);
		for (int i = 0; i < curveDatas.Length; i++)
		{
			AnimationUtility.SetEditorCurve(
				dest,
				curveDatas[i].path,
				curveDatas[i].type,
				curveDatas[i].propertyName,
				curveDatas[i].curve
				);
		}
		return dest;
	}
}
