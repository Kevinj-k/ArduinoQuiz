extends Node2D


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass




func _on_button_a_pressed() -> void:
	get_tree().change_scene_to_file("res://questions/question3.tscn")
	
	



func _on_button_b_pressed() -> void:
	get_tree().change_scene_to_file("res://other screens/loser.tscn")
	
	



func _on_button_c_pressed() -> void:
	get_tree().change_scene_to_file("res://other screens/loser.tscn")
