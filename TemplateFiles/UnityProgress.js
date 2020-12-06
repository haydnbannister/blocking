function UnityProgress(unityInstance, progress) {
  if (!unityInstance.Module)
    return;
  try {
    document.getElementById("loadingBox").style.display = "inline";
    if (progress == 1) {
	  document.getElementById("loadingBox").style.display = "none";
    }
  }
  catch(err) {
	console.log(err)
  }
}