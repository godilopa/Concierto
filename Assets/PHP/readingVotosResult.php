<?php
	$servername = "127.0.0.1";
	$username = "root";
	$dbName = "basePrueba";
		
	$unityId = $_POST["unityId"];

	//Make Connection
	$conn = new mysqli($servername, $username, NULL, $dbName);
	if(!$conn){
		die("connection fail". mysqli_connect_error());
	}
//'".$unityId."'; 
	$sql = "SELECT * FROM preguntas WHERE Id = '".$unityId."'  ";
	$result = mysqli_query($conn, $sql);
	$row = mysqli_fetch_assoc($result);
	
	echo "TransporteId:" ;
	echo $row['TransporteId'];
	echo ";PaisId:" ;
	echo $row['PaisId'];
?>