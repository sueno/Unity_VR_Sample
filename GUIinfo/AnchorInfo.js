#pragma strict

class AnchorInfo{

private static var anchorInfo : AnchorInfo = new AnchorInfo();

private var rightAnchor : boolean = false;
private var leftAnchor : boolean = false;

private function AnchorInfo () {
}

function setRightAnchor(state : boolean) {
	rightAnchor = state;
}

function setLeftAnchor(state : boolean) {
	leftAnchor = state;
}

function getRightAnchor() {
	return rightAnchor;
}

function getLeftAnchor() {
	return leftAnchor;
}

public static function getInstance() {
	return anchorInfo;
}

}