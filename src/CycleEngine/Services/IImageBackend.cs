namespace CycleEngine.Services
{
    public interface IImageBackend : IService
    {
        /// <summary>
        /// Update the content of an image immediately
        /// </summary>
        /// <param name="entity">Key of the entity</param>
        /// <param name="image">Absolute path to the image</param>
        void SetContent(string entity, string image);
        
        /// <summary>
        /// Update the content of an image to another image in gradient
        /// </summary>
        /// <param name="entity">Key of the entity</param>
        /// <param name="startImage">Relative path from project root to the beginning content of the image</param>
        /// <param name="endImage">Relative path from project root to the ending content of the image</param>
        /// <param name="alpha">Process from 0-100 for the transition. <br/>
        /// Alpha for starting image = 100 - alpha <br/>
        /// Alpha for ending image = alpha
        /// </param>
        void TransitionContent(string entity, string startImage, string endImage, double alpha);
    }
}