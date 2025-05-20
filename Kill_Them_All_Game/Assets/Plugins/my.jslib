mergeInto(LibraryManager.library, {

  SetToLeaderboard: function(value) {
    if (window.ysdk) {
      window.ysdk.getLeaderboards().then(lb => {
        lb.setLeaderboardScore("Waves", value)
        .then(() => {
          console.log("Score submitted successfully");
        })
        .catch(err => {
          console.error("Error submitting score", err);
        });
      });
    } else {
      console.error('Yandex SDK не инициализирован');
    }
  },

  OpenSceneByDeviceType: function() {
    if (ysdk.deviceInfo.isDesktop()) {
      SendMessage("StartSceneScript", "OpenScene1");
    } else if (ysdk.deviceInfo.isTablet() || ysdk.deviceInfo.isMobile()) {
      SendMessage("StartSceneScript", "OpenScene2");
    }
  },

  RateGame: function () {

    ysdk.feedback.canReview()
    .then(({ value, reason }) => {
      if (value) {
        ysdk.feedback.requestReview()
        .then(({ feedbackSent }) => {
          console.log(feedbackSent);
        })
      } else {
        console.log(reason)
      }
    })
  },

  ShowAdvEndGame : function() {
    ysdk.adv.showFullscreenAdv({
      callbacks: {
        onClose: function(wasShown) {
          SendMessage("VictoryManager", "ContinueAfterCloseVictory");
        },
        onError: function(error) {
          // some action on error
        }
      }
    })
  },

  ShowAdvCloseDeath : function() {
    ysdk.adv.showFullscreenAdv({
      callbacks: {
        onClose: function(wasShown) {
          SendMessage("PlayerDeathManager", "CloseDeathPanel");
        },
        onError: function(error) {
          // some action on error
        }
      }
    })
  },

  ShowAdvCloseShop : function() {
    ysdk.adv.showFullscreenAdv({
      callbacks: {
        onClose: function(wasShown) {
          SendMessage("GameManager", "CloseShopExtern");
        },
        onError: function(error) {
          // some action on error
        }
      }
    })
  },

  AddCoinsExtern : function() {
    ysdk.adv.showRewardedVideo({
      callbacks: {
        onOpen: () => {
          console.log('Video ad open.');
          SendMessage("AudioManager", "PauseAudioBG");
        },
        onRewarded: () => {
          console.log('Rewarded!');
          SendMessage("ScoreManager", "AddScoreInReward");
        },
        onClose: () => {
          console.log('Video ad closed.');
          SendMessage("AudioManager", "ResumeAudioBG");
        }, 
        onError: (e) => {
          console.log('Error while open video ad:', e);
        }
      }
    })
  },

  InitializeLang: function() {
    if (window.ysdk) { 
      var lang = window.ysdk.environment.i18n.lang; 
      console.log('Setting language to:', lang); 
      SendMessage('Language', 'SetLanguage', lang); 
    } else {
      console.error('Yandex SDK не инициализирован');
    }
  },

});