using System.Collections.Generic;
using System.Threading;
using BingoGame.Dto;
using Cysharp.Threading.Tasks;
using Postgrest;
using Supabase.Gotrue;
using Supabase.Interfaces;
using Supabase.Realtime;
using Supabase.Storage;
using Unity.VisualScripting;

namespace BingoGame.Services
{
    internal class SupabaseInstance
    {
        private readonly ISupabaseClient<User, Session, RealtimeSocket, RealtimeChannel, Bucket, FileObject> _supabase;

        public SupabaseInstance(ISupabaseClient<User, Session, RealtimeSocket, RealtimeChannel, Bucket, FileObject> supabase)
        {
            _supabase = supabase;
        }

        public async UniTask<List<EpisodeModel>> GetEpisodesAsync(CancellationToken cancellationToken)
        {
            var episodes = await _supabase.From<EpisodeModel>().Get(cancellationToken);
            return episodes.Models;
        }

        public async UniTask InsertNewGame(GameModel gameModel, CancellationToken cancellationToken)
        {
            await _supabase.From<GameModel>().Insert(gameModel, null, cancellationToken);
        }
    }
}