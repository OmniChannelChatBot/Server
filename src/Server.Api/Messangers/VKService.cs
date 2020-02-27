using Server.Api.Models;
using System;
using System.Collections.Generic;
using VkNet;
using VkNet.Enums.SafetyEnums;
using VkNet.Exception;
using VkNet.Model;
using VkNet.Model.RequestParams;

namespace Server.Api.Messangers
{
    public class VKService
    {
        public void CreateVKService(List<MessengerToken> tokens)
        {
            //TODO: подумать как обрабатывать несколько аккаунтов и несколько групп в них
            foreach (var token in tokens)
            {
                var api = new VkApi();
                api.Authorize(new ApiAuthParams() { AccessToken = token.Token });
                foreach (var group in token.Groups)
                {
                    var groupId = ulong.Parse(group);
                    var s = api.Groups.GetLongPollServer(groupId);

                    bool FirstMessage = true;

                    int CurrentTS = 0;

                    while (true)
                    {
                        try
                        {
                            var poll = api.Groups.GetBotsLongPollHistory(
                                new BotsLongPollHistoryParams()
                                {
                                    Server = s.Server,
                                    Ts = FirstMessage ? s.Ts : CurrentTS.ToString(),
                                    Key = s.Key,
                                    Wait = 2
                                });
                            if (poll?.Updates == null)
                            {
                                continue;
                            }

                            foreach (var a in poll.Updates)
                            {
                                if (a.Type == GroupUpdateType.MessageNew)
                                {
                                    FirstMessage = false;

                                    // TODO: сделать отправку на наш сервер
                                    //api.Messages.Send(new MessagesSendParams()
                                    //{
                                    //    UserId = a.Message.UserId,
                                    //    //Message = a.Message.Body,
                                    //    Message = "sex test message 100500",
                                    //    RandomId = (int)a.Message.RandomId,
                                    //    PeerId = a.Message.PeerId
                                    //});

                                    // помечаем прочитанным
                                    var sex = api.Messages.MarkAsRead(a.Message.PeerId.ToString(), a.Message.Id);

                                    // проставляем номер ивента, с которого стоит читать
                                    // этот стейт нужно сохранять, иначе будем терять сообщения
                                    CurrentTS = int.Parse(poll.Ts);
                                }
                            }
                        }
                        catch (LongPollException exception)
                        {
                            if (exception is LongPollOutdateException outdateException)
                            {
                                //server.Ts = outdateException.Ts;
                            }
                            else
                            {
                                s = api.Groups.GetLongPollServer(groupId);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
            }
        }
    }
}
