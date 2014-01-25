#pragma strict

private var anchorInfo = AnchorInfo.getInstance();

function Start () {

}

function Update () {
    	guiText.text = "R : "+(anchorInfo.getRightAnchor()?"T":"F")+"  L : "+(anchorInfo.getLeftAnchor()?"T":"F");
}