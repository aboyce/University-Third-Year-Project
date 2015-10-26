using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace ConnectingToTFS
{
    class ClientObjectModel
    {
        public void LatestCommit(Uri uri)
        {
            TfsTeamProjectCollection tfs = new TfsTeamProjectCollection(uri, new TfsClientCredentials());

            var vcs = tfs.GetService<VersionControlServer>();
            var latestChangeId = vcs.GetLatestChangesetId();
            var latestChangeSet = vcs.GetChangeset(latestChangeId);

            Console.WriteLine("Latest Changeset: {0}", latestChangeId);
            Console.WriteLine("Committer: {0}", latestChangeSet.CommitterDisplayName);
            Console.WriteLine("Comment: {0}", latestChangeSet.Comment);
        }
    }
}
