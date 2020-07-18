<?php
	$servername = "127.0.0.1";
	$username = "julian";
	$password = "123456";
	$dbName = "basePrueba";
	
	//Unity variables
	$unityId = $_POST["unityId"];
    $transporteId = $_POST["transporteId"];
	$paisId = $_POST["paisId"];
	
	//Make Connection
	$conn = new mysqli($servername, $username, $password, $dbName);
	
	if(!$conn){
		die("connection fail". mysqli_connect_error());
	}
	
	$sql = "UPDATE preguntas
		SET TransporteId = '".$transporteId."', PaisId = '".$paisId."'
		WHERE Id = '".$unityId."' ";
			
	$result = mysqli_query($conn, $sql);
	 
	 //$sql ="SELECT Votos FROM votacion WHERE IdRonda = '".$unityId."' AND IdPregunta = '".$unityVotacionId."' ";
	 //$result = mysqli_query($conn, $sql);
	 //echo $result->fetch_object()->Votos
?>