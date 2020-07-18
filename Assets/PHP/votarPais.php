<?php
	$servername = "127.0.0.1";
	$username = "root";
	$dbName = "basePrueba";
	
	//Unity variables
    $unityPreguntaId = $_POST["unityPreguntaId"];
	$unityPaisId = $_POST["unityPaisId"];
	
	//Make Connection
	$conn = new mysqli($servername, $username, NULL, $dbName);
	
	if(!$conn){
		die("connection fail". mysqli_connect_error());
	}
	
	$sql = "UPDATE votacion
		SET Votos2 = Votos2 + 1
		WHERE IdPregunta = '".$unityPreguntaId."' AND IdPais = '".$unityPaisId."' ";
			
	$result = mysqli_query($conn, $sql);
	 
	 //$sql ="SELECT Votos FROM votacion WHERE IdRonda = '".$unityId."' AND IdPregunta = '".$unityVotacionId."' ";
	 //$result = mysqli_query($conn, $sql);
	 //echo $result->fetch_object()->Votos
?>