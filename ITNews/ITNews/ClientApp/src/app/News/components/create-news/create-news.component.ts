import { AuthService } from 'src/app/Shared/services/auth.service';
import { PhotoService } from './../../../Shared/services/photo-service';
import { Category } from './../../../Shared/models/category';
import { NewsService } from './../../services/news.service';
import { TagService } from './../../../Shared/services/tag.service';
import { CategoryService } from './../../../Shared/services/category.service';
import { Component, OnInit, ElementRef, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Tag } from 'src/app/Shared/models/tag';
import { Subject, Observable, BehaviorSubject } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap, catchError, ignoreElements } from 'rxjs/operators';
import { of, forkJoin } from 'rxjs/index';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MarkdownService } from 'ngx-markdown';
import { EditorInstance, EditorOption } from 'angular-markdown-editor/lib/angular-markdown-editor';
import { faFileDownload } from '@fortawesome/free-solid-svg-icons';
import { SaveNews } from '../../models/SaveNews';
@Component({
  selector: 'app-create-news',
  templateUrl: './create-news.component.html',
  styleUrls: ['./create-news.component.css']
})
export class CreateNewsComponent implements OnInit {
  news: SaveNews;
  newsId: number;
  categories: Category[] = [];
  // string for searching tag on the server
  searchValue;
  // file input element
  @ViewChild('fileInput') fileInput: ElementRef;
  photoPath: string;
  photo;
  fileImageDownload = faFileDownload;

  // makrdown editor
  bsEditorInstance: EditorInstance;
  showEditor = true;
  templateForm: FormGroup;
  editorOptions: EditorOption;

  // diffrent modes settings
  editMode = false;
  pageName = 'Creating News';
  saveButtonName = 'Create News';
  // for forkJoin
  sources = <any>[];



  constructor(
    private categoryService: CategoryService,
    private tagService: TagService,
    private newsService: NewsService,
    private activatedRoute: ActivatedRoute,
    private authService: AuthService,
    private router: Router,
    private fb: FormBuilder,
    private markdownService: MarkdownService,
    private photoService: PhotoService
  ) {
    this.news = <SaveNews>{};
    this.news.tags = [];
    this.news.categories = [];
    this.news.markDown = `### News `;
  }

  ngOnInit() {

    // MarkDownEdotor settings
    this.editorOptions = {
      autofocus: false,
      iconlibrary: 'fa',
      savable: false,
      onShow: (e) => this.bsEditorInstance = e,
      parser: (val) => this.parse(val)
    };
    this.buildForm(this.news.markDown);

    this.sources = [
      this.categoryService.getCategories()
    ];
    // Gettings of category and Tags
    this.newsId = +this.activatedRoute.snapshot.paramMap.get('id');
    if (this.newsId) {
      this.sources.push(this.newsService.getForEdit(this.newsId));
      this.saveButtonName = 'Edit News';
      this.pageName = 'Editing News';
      this.editMode = true;

    }
    forkJoin(this.sources)
      .subscribe(data => {
        console.log('categories: ' + JSON.stringify(data[0]));
        console.log('news -> ' + JSON.stringify(data[1]));
        this.categories = <Category[]>data[0];
        if (this.newsId) {
          this.news = <SaveNews>data[1];
        }
      }, error => console.log(error));

  }
  showForm(f) {
    console.log(f);
  }
  containCategory(id: number) {
    return this.news.categories.some(cat => cat.id === id);
  }
  tagSource = (keyword: any) => {
    if (keyword) {
      return this.tagService.getTagsByPart(keyword)
        .pipe(
          debounceTime(300),
          distinctUntilChanged());
    } else {
      return of(<Tag[]>([]));
    }
  }
  // display settings for autocomplete input
  myValueFormatter(data: Tag): string {
    return `(${data.id}) ${data.name}`;
  }

  deleteTag(tagId: number) {
    console.log('tagId->' + tagId);
    const ind = this.news.tags.findIndex(t => t.id === tagId);
    this.news.tags.splice(ind, 1);
  }
  addTag() {
    console.log('searchValue -> ' + this.searchValue);

    if (this.searchValue !== '' && !this.news.tags.some(t => t.name === this.searchValue.name)) {
      this.news.tags.push(this.searchValue);
    }
    this.searchValue = null;
  }
  onNewTag($event) {
    if ($event.keyCode === 13 && this.searchValue) {
      console.log($event);
      if (!this.news.tags.some(t => t.name === this.searchValue )) {
        this.news.tags.push({ id: 0, name: this.searchValue });
        this.searchValue = null;
      }
    }
  }

  toggleCategory(id, $event) {
    if ($event.target.checked) {
      const elemToAdd = this.categories.find(c => c.id === id);
      this.news.categories.push(elemToAdd);
    } else {
      const indToRemove = this.news.categories.findIndex(c => c.id === id);
      this.news.categories.splice(indToRemove, 1);
    }
    console.log(JSON.stringify(this.news.categories));
  }

  sendNews() {
    this.news.content = this.markdownService.compile(this.news.markDown, true);
    if (this.newsId) {
      this.news.modifiedBy = this.authService.getUserId();
      this.newsService.changeNews(this.news, this.newsId)
        .subscribe(res => {
          console.log(res);
          this.router.navigate(['/news/success-edit/', this.newsId]);
        });
    } else {
      this.news.userId = this.authService.getUserId();
      this.newsService.createNews(this.news)
      .subscribe(res => {
        console.log('News has been created');
        console.log(res);
        console.log(JSON.stringify(this.news));
        this.router.navigate(['/news/success-create/', res.id]);
      });
    }
  }

  buildForm(markdownText) {
    this.templateForm = this.fb.group({
      body: [markdownText],
      isPreview: [true]
    });
  }

  /** highlight all code found, needs to be wrapped in timer to work properly */
  highlight() {
    setTimeout(() => {
      this.markdownService.highlight();
    });
  }

  hidePreview() {
    if (this.bsEditorInstance && this.bsEditorInstance.hidePreview) {
      this.bsEditorInstance.hidePreview();
    }
  }

  showFullScreen(isFullScreen: boolean) {
    if (this.bsEditorInstance && this.bsEditorInstance.setFullscreen) {
      this.bsEditorInstance.showPreview();
      this.bsEditorInstance.setFullscreen(isFullScreen);
    }
  }

  parse(inputValue: string) {
    const markedOutput = this.markdownService.compile(inputValue.trim());
    this.highlight();

    return markedOutput;
  }

  uploadPhoto(photo) {
    this.photoService.createNewsPhoto(photo.files[0])
    .subscribe(res => {
      console.log('file was downloaded to path: ' + res.fileName);
      this.photoPath = res.fileName;
    }, error => console.log(error));
  }
  getBaseUrl() {
    return document.getElementsByTagName('base')[0].href;
  }
}
