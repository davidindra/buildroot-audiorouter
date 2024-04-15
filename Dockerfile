FROM davidindracz/buildroot-base:latest AS build

WORKDIR /buildroot

# this .config was obtained by running `make raspberrypi3_defconfig` and 
COPY buildroot.config .config

COPY buildroot-overlay /buildroot-overlay

RUN chmod 644 /buildroot-overlay/etc/ssh/sshd_config
RUN chmod u+x /buildroot-overlay/etc/init.d/S02modules
RUN chmod u+x /buildroot-overlay/etc/sysconfig/functions

RUN make all

FROM scratch AS export

COPY --from=build /buildroot/output/images/sdcard.img .

ENTRYPOINT [ "/usr/bin/echo", "This image is not meant to be run. The resulting buildroot image is being produced as part of the docker build step." ]

# build: docker build -t buildroot-audiorouter --output out .