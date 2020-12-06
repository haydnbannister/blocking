var IsMobilePlugin = {
   IsMobile: function()
   {
      return UnityLoader.SystemInfo.mobile;
   }
};  
mergeInto(LibraryManager.library, IsMobilePlugin);