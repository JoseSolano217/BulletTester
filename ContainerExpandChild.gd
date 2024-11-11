extends ScrollContainer

func _process(delta: float) -> void:
	if (get_child_count() > 0):
		var child = get_child(0)
		if (child is Control):
			child.size = size
