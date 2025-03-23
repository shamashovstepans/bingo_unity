using System;
using System.Threading;
using UnityEngine;

namespace BingoGame.Services.Episodes
{
    internal class EpisodesController
    {
        private readonly SupabaseService _supabaseService;
        private readonly EpisodesView _episodesView;

        public EpisodesController(SupabaseService supabaseService, EpisodesView episodesView)
        {
            _supabaseService = supabaseService;
            _episodesView = episodesView;

            Init();
        }
        
        private async void Init()
        {
            try
            {
                var result = await SupabaseService.SupabaseInstance.GetEpisodesAsync(CancellationToken.None);
            
                foreach (var episode in result)
                {
                    _episodesView.CreateEpisode(episode);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}