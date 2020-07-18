<?php
	$servername = "127.0.0.1";
	$username = "julian";
	$password = "123456";
	$dbName = "basePrueba";
	
	$unityVotosId = $_POST["unityVotosId"];

	//Make Connection
	$conn = new mysqli($servername, $username, $password, $dbName);
	
	if(!$conn){
		die("connection fail". mysqli_connect_error());
	}

	$sql ="SELECT Votos, IdTransporte FROM votacion WHERE IdPregunta = '".$unityVotosId."' ";
	$result = mysqli_query($conn, $sql);	 

	while($row = mysqli_fetch_array($result)) {
		echo "IdTransporte:" .$row['IdTransporte']. "|Votos:" .$row['Votos']. ";" ;
	}
	
	$sql ="SELECT Votos2, IdPais FROM votacion WHERE IdPregunta = '".$unityVotosId."' ";
	$result = mysqli_query($conn, $sql);	 

	while($row = mysqli_fetch_array($result)) {
		echo "IdPais:" .$row['IdPais']. "|Votos:" .$row['Votos2']. ";" ;
	}
	
			//while($row = mysql_fetch_assoc($result)) {
		//echo "Votos:" .$row['Votos']. ";";
	//}
?>