#pragma strict

class Score {

static var score : int = 0;

function Score () {
}

static function add(i : int) {
	score += i;
}

static function setScore(i : int) {
	score = i;
}

}