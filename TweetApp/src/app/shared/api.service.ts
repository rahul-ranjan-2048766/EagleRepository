import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { LoginApiService } from '../signin/login-api.service';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(
    private client : HttpClient,
    private loginApi : LoginApiService
  ) { }

  RegisterUser(data : any) {
    return this.client.post(environment.RestApi + "User/Add", data);
  }

  AuthenticateUser(data : any) {
    return this.client.post(environment.RestApi + "Authenticate/Authenticate", data);
  }

  GetUserByLoginId(data : any) {
    const token = localStorage.getItem(this.loginApi.USER_TOKEN);
    const reqHeaders = new HttpHeaders({ "Authorization" : "Bearer " + token });
    return this.client.get(environment.RestApi + "Profile/GetUser/" + data, { headers: reqHeaders });
  }

  UpdateUser(data : any) {
    const token = localStorage.getItem(this.loginApi.USER_TOKEN);
    const reqHeaders = new HttpHeaders({ "Authorization" : "Bearer " + token });
    return this.client.put(environment.RestApi + "User/Update", data, { headers: reqHeaders });
  }

  GetUsers() {
    const token = localStorage.getItem(this.loginApi.USER_TOKEN);
    const reqHeaders = new HttpHeaders({ "Authorization" : "Bearer " + token });
    return this.client.get(environment.RestApi + "User/Get", { headers: reqHeaders });
  }

  SearchUsers(data : any) {
    const token = localStorage.getItem(this.loginApi.USER_TOKEN);
    const reqHeaders = new HttpHeaders({ "Authorization" : "Bearer " + token });
    return this.client.get(environment.RestApi + "UserSearch/SearchUser/" + data, { headers: reqHeaders });
  }

  SaveTweet(data : any) {
    const token = localStorage.getItem(this.loginApi.USER_TOKEN);
    const reqHeaders = new HttpHeaders({ "Authorization" : "Bearer " + token });
    return this.client.post(environment.TweetApi + "Tweet/Add", data, { headers: reqHeaders });
  }

  GetTweets() {
    const token = localStorage.getItem(this.loginApi.USER_TOKEN);
    const reqHeaders = new HttpHeaders({ "Authorization" : "Bearer " + token });
    return this.client.get(environment.TweetApi + "TweetAndComments/GetTweetsAndComments", { headers: reqHeaders });
  }

  SaveComment(data : any) {
    const token = localStorage.getItem(this.loginApi.USER_TOKEN);
    const reqHeaders = new HttpHeaders({ "Authorization" : "Bearer " + token });
    return this.client.post(environment.TweetApi + "Comment/Add", data, { headers: reqHeaders });
  }

  PersonalTweets(data : any) {
    const token = localStorage.getItem(this.loginApi.USER_TOKEN);
    const reqHeaders = new HttpHeaders({ "Authorization" : "Bearer " + token });
    return this.client.get(environment.TweetApi + "PersonalTweets/Get/" + data, { headers: reqHeaders });
  }
}
