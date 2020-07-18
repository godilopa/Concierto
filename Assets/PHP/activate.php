<?php
	$servername = "127.0.0.1";
	$username = "julian";
	$password = "123456";
	$dbName = "basePrueba";
	
	//Unity variables
	$valor = $_POST["valor"];
    $unityId = $_POST["unityId"];
	
	//Make Connection
	$conn = new mysqli($servername, $username, $password, $dbName);
	
	if(!$conn){
		die("connection fail". mysqli_connect_error());
	}
	
	$sql = "UPDATE actualid SET ActualId = '".$unityId."'; ";
	$result = mysqli_query($conn, $sql);
	
	$sql = "UPDATE preguntas SET Activada = '".$valor."' WHERE Id = '".$unityId."' ";			
	$result = mysqli_query($conn, $sql);
	
?>