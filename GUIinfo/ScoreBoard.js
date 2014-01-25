#pragma strict

private var s_elapsed : float = 0;
var timelimit : float = 0;

private var total_score : int = -1;
    
function Start () {
}

function Update () {
	if (s_elapsed<timelimit) {
    	s_elapsed += Time.deltaTime;
    	guiText.text = "Time : " + (parseInt(timelimit)-parseInt(s_elapsed))+"\nScore : "+Score.score;
    } else {
    	if (total_score<0) {
    		total_score = Score.score;
    	}
    	guiText.text = "Your Score : "+total_score+" !";
    }
}