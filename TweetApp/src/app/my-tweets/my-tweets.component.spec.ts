import { HttpClientModule } from '@angular/common/http';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { ApiService } from '../shared/api.service';
import { LoginApiService } from '../signin/login-api.service';

import { MyTweetsComponent } from './my-tweets.component';

describe('MyTweetsComponent', () => {
  let component: MyTweetsComponent;
  let fixture: ComponentFixture<MyTweetsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MyTweetsComponent ],
      imports: [
        RouterTestingModule,
        HttpClientModule
      ],
      providers: [
        LoginApiService,
        ApiService
      ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MyTweetsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
