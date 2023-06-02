# Gazel.Cli in Docker

- [Install docker](https://docs.docker.com/engine/install/)
- Create a docker image using `Dockerfile` under
  `./samples/gazel-cli-in-docker` directory
  ```
  docker build -t gazel-cli .
  ```
- Check if docker image is created correctly
  ```
  docker run gazel-cli g --version
  ```
- Now use gazel cli within this image;
  ```
  docker run --rm -v {host-path}:{container-path} gazel-cli \
    g codegen http://sample-service.inventiv.com.tr/ -o {container-path}
  ```
  - `--rm` removes container after it finishes execution
  - `-v {host-path}:{container-path}` mounts `{host-path}` to
    `{container-path}`. `{host-path}` must be an absolute path.
- Generated output should be be in the `{host-path}` directory.
