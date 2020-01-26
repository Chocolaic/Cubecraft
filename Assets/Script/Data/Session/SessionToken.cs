using System;
using System.Collections.Generic;
using System.IO;

[Serializable]
public class SessionToken
{
    public string accessToken;
    public string clientToken;
    public SelectedProfile selectedProfile;
    public List<AvailableProfile> availableProfiles;
    [Serializable]
    public class SelectedProfile
    {
        public string name;
        public string id;
    }
    [Serializable]
    public class AvailableProfile
    {
        public string name;
        public string id;
    }
}
