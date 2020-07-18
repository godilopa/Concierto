<?php
	$servername = "127.0.0.1";
	$username = "root";
	$dbName = "basePrueba";

	//Make Connection
	 $conn = new mysqli($servername, $username, NULL, $dbName);
	 
	if(!$conn){
		die("connection fail". mysqli_connect_error());
	}

	$sql = "SELECT ActualId FROM actualid ";
	$result = mysqli_query($conn, $sql);
	echo $result->fetch_object()->ActualId;
?>