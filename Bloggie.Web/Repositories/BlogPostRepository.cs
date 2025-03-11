﻿using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Repositories
{


    public class BlogPostRepository : IBlogPostRepository
    {

        private readonly BloggieDbContext _bloggieDbContext;
        public BlogPostRepository(BloggieDbContext bloggieDbContext)
        {
            _bloggieDbContext = bloggieDbContext;
        }

        public async Task<BlogPost> AddAsync(BlogPost blogPost)
        {
            await _bloggieDbContext.AddAsync(blogPost);
            await _bloggieDbContext.SaveChangesAsync();

            return blogPost;
        }


        public async Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return await _bloggieDbContext.BlogPosts.Include(x => x.Tags).ToListAsync();
        }

        public async Task<BlogPost?> GetAsync(Guid id)
        {
          return await  _bloggieDbContext.BlogPosts.Include(x => x.Tags).FirstOrDefaultAsync(x => x.Id == id);    
        }

        public async Task<BlogPost?> UpdateAsync(BlogPost blogPost)
        {
          var existingBlog = await _bloggieDbContext.BlogPosts.Include(x => x.Tags)
                .FirstOrDefaultAsync(x => x.Id == blogPost.Id);
            if (existingBlog != null) 
            {
                existingBlog.Id = blogPost.Id;
                existingBlog.Heading = blogPost.Heading;
                existingBlog.PageTitle = blogPost.PageTitle;
                existingBlog.Content = blogPost.Content;
                existingBlog.ShortDescription = blogPost.ShortDescription;
                existingBlog.Author = blogPost.Author;
                existingBlog.FeaturedImageUrl = blogPost.FeaturedImageUrl;
                existingBlog.UrlHandle = blogPost.UrlHandle;
                existingBlog.Visible = blogPost.Visible;
                existingBlog.PublishedDate = blogPost.PublishedDate;
                existingBlog.Tags = blogPost.Tags;  

                await _bloggieDbContext.SaveChangesAsync();
                return existingBlog;
            }
            return null;
        }


        public async Task<BlogPost?> DeleteAsync(Guid id)
        {
            var existingBlogPost = await _bloggieDbContext.BlogPosts.FindAsync(id);

            if (existingBlogPost != null)
            {
                _bloggieDbContext.BlogPosts.Remove(existingBlogPost);
                await _bloggieDbContext.SaveChangesAsync();

                return existingBlogPost;
            }
            return null;
        }

        public async Task<BlogPost?> GetByUrlHandleAsync(string urlHandle)
        {
           return await _bloggieDbContext.BlogPosts.Include( x=> x.Tags)
                .FirstOrDefaultAsync(x => x.UrlHandle == urlHandle);
        }
    }
}
